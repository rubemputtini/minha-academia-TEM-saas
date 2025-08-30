using FluentAssertions;
using Moq;
using Stripe;
using Stripe.Checkout;
using MinhaAcademiaTEM.Infrastructure.Services;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class StripeCheckoutSessionReaderTests
{
    private readonly Mock<IStripeClient> _stripe = new();

    private StripeCheckoutSessionReader CreateSut() => new(_stripe.Object);

    [Fact]
    public async Task GetPrefillAsync_Should_Throw_When_SessionId_Empty()
    {
        var sut = CreateSut();
        Func<Task> act = () => sut.GetPrefillAsync("");
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*session_id*");
    }

    [Fact]
    public async Task GetPrefillAsync_Should_Throw_When_Status_Not_Complete()
    {
        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions/sess_1")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Id = "sess_1", Status = "open", Mode = "subscription", CustomerId = "cus" });

        var sut = CreateSut();
        var act = () => sut.GetPrefillAsync("sess_1");
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetPrefillAsync_Should_Throw_When_Mode_Not_Subscription()
    {
        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions/sess_2")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Id = "sess_2", Status = "complete", Mode = "payment", CustomerId = "cus" });

        var sut = CreateSut();
        var act = () => sut.GetPrefillAsync("sess_2");
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetPrefillAsync_Should_Throw_When_No_Customer()
    {
        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions/sess_3")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Session { Id = "sess_3", Status = "complete", Mode = "subscription", CustomerId = null });

        var sut = CreateSut();
        var act = () => sut.GetPrefillAsync("sess_3");
        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task GetPrefillAsync_Should_Map_All_Fields_On_Success()
    {
        var session = new Session
        {
            Id = "sess_ok",
            Status = "complete",
            Mode = "subscription",
            CustomerId = "cus_123",
            SubscriptionId = "sub_123",
            ClientReferenceId = "coach_123",
            Metadata = new Dictionary<string, string> { ["app_plan"] = "Unlimited" },
            CustomerDetails = new SessionCustomerDetails
            {
                Name = "Coach Name",
                Email = "coach@example.com",
                Phone = "+351999999999",
                Address = new Address
                {
                    Line1 = "Rua X",
                    Line2 = "Ap 10",
                    City = "Porto",
                    State = "PT",
                    Country = "PT",
                    PostalCode = "4000-000"
                }
            }
        };

        _stripe.Setup(c => c.RequestAsync<Session>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/checkout/sessions/sess_ok")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(session);

        var sut = CreateSut();
        var res = await sut.GetPrefillAsync("sess_ok");

        res.Name.Should().Be("Coach Name");
        res.Email.Should().Be("coach@example.com");
        res.PhoneNumber.Should().Be("+351999999999");
        res.Address.Street.Should().Be("Rua X");
        res.Address.Complement.Should().Be("Ap 10");
        res.Address.City.Should().Be("Porto");
        res.Address.State.Should().Be("PT");
        res.Address.Country.Should().Be("PT");
        res.Address.PostalCode.Should().Be("4000-000");
        res.SubscriptionPlan.Should().Be("Unlimited");
        res.StripeCustomerId.Should().Be("cus_123");
        res.StripeSubscriptionId.Should().Be("sub_123");
        res.ClientReferenceId.Should().Be("coach_123");
    }
}