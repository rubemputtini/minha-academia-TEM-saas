using FluentAssertions;
using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;
using Moq;
using Stripe;
using Stripe.Checkout;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class StripeCheckoutSessionsServiceTests
{
    private readonly Mock<IStripeClient> _stripe = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IGymRepository> _gyms = new();

    private readonly EntityLookup _lookup;
    private readonly IOptions<StripeApiConfiguration> _opts;

    private readonly Guid _userId = Guid.NewGuid();

    public StripeCheckoutSessionsServiceTests()
    {
        _lookup = new EntityLookup(_users.Object, _coaches.Object, _gyms.Object);

        _opts = Options.Create(new StripeApiConfiguration
        {
            SignupSuccessUrl = "https://app.example.com/signup/success",
            SuccessUrl = "https://app.example.com/checkout/success",
            CancelUrl = "https://app.example.com/checkout/cancel",
            PriceIds = new Dictionary<string, string>
            {
                ["Basic"] = "price_basic",
                ["Unlimited"] = "price_unlimited"
            }
        });

        _currentUser.Setup(x => x.GetUserId()).Returns(_userId);
    }

    private StripeCheckoutSessionsService CreateSut() =>
        new(_opts, _coaches.Object, _lookup, _currentUser.Object, _stripe.Object);

    [Fact]
    public async Task CreateSignupAsync_Should_Return_Url_On_Success()
    {
        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Url = "https://checkout.stripe.com/s/abc" });

        var sut = CreateSut();

        var url = await sut.CreateSignupAsync(SubscriptionPlan.Basic, "idem-1");

        url.Should().Be("https://checkout.stripe.com/s/abc");

        _stripe.Verify(c => c.RequestAsync<Session>(
            HttpMethod.Post,
            It.Is<string>(u => u.Contains("/v1/checkout/sessions")),
            It.IsAny<BaseOptions>(),
            It.Is<RequestOptions>(r => r.IdempotencyKey == "idem-1"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateSignupAsync_Should_Throw_When_Stripe_Url_Empty()
    {
        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Url = null });

        var sut = CreateSut();

        var act = () => sut.CreateSignupAsync(SubscriptionPlan.Basic, "idem-2");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateCoachSubscriptionAsync_Should_Throw_When_Coach_Already_Has_Subscription()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByUserIdAsync(_userId)).ReturnsAsync(coach);

        var sut = CreateSut();
        var act = () => sut.CreateCoachSubscriptionAsync(SubscriptionPlan.Basic, "idem-3");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateCoachSubscriptionAsync_Should_Return_Url_On_Success()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "");

        _coaches.Setup(r => r.GetByUserIdAsync(_userId)).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Url = "https://checkout.stripe.com/s/xyz" });

        var sut = CreateSut();
        var url = await sut.CreateCoachSubscriptionAsync(SubscriptionPlan.Unlimited, "idem-4");

        url.Should().Be("https://checkout.stripe.com/s/xyz");

        _stripe.Verify(c => c.RequestAsync<Session>(
            HttpMethod.Post,
            It.Is<string>(u => u.Contains("/v1/checkout/sessions")),
            It.IsAny<BaseOptions>(),
            It.Is<RequestOptions>(r => r.IdempotencyKey == "idem-4"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ProcessCheckoutCompletedAsync_Should_NoOp_When_ClientReference_Invalid()
    {
        var sut = CreateSut();

        await sut.ProcessCheckoutCompletedAsync(new CheckoutSessionCompletedRequest
        {
            ClientReferenceId = "not-a-guid"
        });

        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }

    [Fact]
    public async Task ProcessCheckoutCompletedAsync_Should_Update_Coach_With_Metadata_Plan()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("", "");
        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);
        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        var req = new CheckoutSessionCompletedRequest
        {
            ClientReferenceId = coach.Id.ToString(),
            CustomerId = "cus_new",
            SubscriptionId = "sub_new",
            Metadata = new Dictionary<string, string> { ["app_plan"] = "Unlimited" }
        };

        var sut = CreateSut();

        await sut.ProcessCheckoutCompletedAsync(req);

        coach.StripeCustomerId.Should().Be("cus_new");
        coach.StripeSubscriptionId.Should().Be("sub_new");
        coach.SubscriptionPlan.Should().Be(SubscriptionPlan.Unlimited);
        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Active);
        coach.SubscriptionEndAt.Should().BeNull();

        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task ProcessCheckoutCompletedAsync_Should_Keep_Existing_Ids_When_Not_Provided()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_old", "sub_old");
        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);
        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        var req = new CheckoutSessionCompletedRequest
        {
            ClientReferenceId = coach.Id.ToString(),
            // sem CustomerId e SubscriptionId
            Metadata = new Dictionary<string, string> { ["app_plan"] = "Basic" }
        };

        var sut = CreateSut();

        await sut.ProcessCheckoutCompletedAsync(req);

        coach.StripeCustomerId.Should().Be("cus_old");
        coach.StripeSubscriptionId.Should().Be("sub_old");
        coach.SubscriptionPlan.Should().Be(SubscriptionPlan.Basic);
        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Active);

        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }
}