using FluentAssertions;
using Moq;
using Stripe;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class AdminSubscriptionServiceTests
{
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IStripeClient> _stripe = new();

    private readonly EntityLookup _lookup;
    private readonly AdminSubscriptionService _sut;

    public AdminSubscriptionServiceTests()
    {
        _lookup = new EntityLookup(_users.Object, _coaches.Object, _gyms.Object);
        _sut = new AdminSubscriptionService(_lookup, _coaches.Object, _stripe.Object);
    }

    [Fact]
    public async Task CancelNowAsync_Should_Cancel_On_Stripe_Update_Coach_And_Return_Response()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Delete,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription { Id = "sub_123" });

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        var result = await _sut.CancelNowAsync(coach.Id);

        result.CoachId.Should().Be(coach.Id);
        result.SubscriptionStatus.Should().Be(SubscriptionStatus.Canceled);
        coach.SubscriptionEndAt.Should().NotBeNull();
        coach.StripeSubscriptionId.Should().BeNull();

        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
        _stripe.VerifyAll();
    }

    [Fact]
    public async Task CancelNowAsync_Should_Throw_When_SubscriptionId_Missing()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "");

        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);

        var act = () => _sut.CancelNowAsync(coach.Id);

        await act.Should().ThrowAsync<InvalidOperationException>();

        _stripe.Verify(c => c.RequestAsync<Subscription>(
            It.IsAny<HttpMethod>(), It.IsAny<string>(),
            It.IsAny<BaseOptions>(), It.IsAny<RequestOptions>(),
            It.IsAny<CancellationToken>()), Times.Never);

        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }

    [Fact]
    public async Task CancelNowAsync_Should_Not_Persist_When_Stripe_Fails()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Delete,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(new StripeException("network error"));

        var act = () => _sut.CancelNowAsync(coach.Id);

        await act.Should().ThrowAsync<StripeException>();
        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }
}