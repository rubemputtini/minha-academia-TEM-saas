using FluentAssertions;
using Microsoft.Extensions.Configuration;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;
using Moq;
using Stripe;
using Stripe.BillingPortal;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class StripeBillingPortalServiceTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IStripeClient> _stripe = new();
    private readonly Mock<ICurrentUserService> _currentUser = new();

    private readonly EntityLookup _lookup;
    private readonly IConfiguration _cfg;

    private readonly Guid _userId = Guid.NewGuid();

    public StripeBillingPortalServiceTests()
    {
        _lookup = new EntityLookup(_users.Object, _coaches.Object, _gyms.Object);

        _cfg = new ConfigurationBuilder()
            .AddInMemoryCollection(new[]
            {
                new KeyValuePair<string, string>("AppSettings:FrontendUrl", "https://app.example.com"),
            }!)
            .Build();

        _currentUser.Setup(x => x.GetUserId()).Returns(_userId);
    }

    private StripeBillingPortalService CreateSut() => new(_lookup, _currentUser.Object, _cfg, _stripe.Object);

    [Fact]
    public async Task CreateCustomerPortalSessionAsync_Should_Throw_When_Coach_Has_No_StripeCustomer()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("", "sub_123");

        _coaches.Setup(r => r.GetByUserIdAsync(_userId)).ReturnsAsync(coach);

        var sut = CreateSut();

        var act = () => sut.CreateCustomerPortalSessionAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task CreateCustomerPortalSessionAsync_Should_Throw_When_Stripe_Returns_Empty_Url()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByUserIdAsync(_userId)).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/billing_portal/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Url = null });

        var sut = CreateSut();

        var act = () => sut.CreateCustomerPortalSessionAsync();

        await act.Should().ThrowAsync<InvalidOperationException>();

        _stripe.Verify(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/billing_portal/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task CreateCustomerPortalSessionAsync_Should_Return_Url_On_Success()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByUserIdAsync(_userId)).ReturnsAsync(coach);

        var expectedUrl = "https://billing.stripe.com/session/abc";
        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/billing_portal/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Url = expectedUrl });

        var sut = CreateSut();

        var url = await sut.CreateCustomerPortalSessionAsync();

        url.Should().Be(expectedUrl);

        _stripe.Verify(c => c.RequestAsync<Session>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/billing_portal/sessions")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }
}