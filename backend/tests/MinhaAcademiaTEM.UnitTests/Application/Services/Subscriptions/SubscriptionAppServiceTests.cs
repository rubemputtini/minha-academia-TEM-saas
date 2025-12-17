using FluentAssertions;
using Moq;
using Stripe;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class SubscriptionAppServiceTests
{
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IStripeClient> _stripe = new();
    private readonly Mock<ISubscriptionSummaryReader> _subscription = new();

    private readonly EntityLookup _lookup;
    private readonly SubscriptionAppService _sut;

    public SubscriptionAppServiceTests()
    {
        _lookup = new EntityLookup(_users.Object, _coaches.Object, _gyms.Object);
        _sut = new SubscriptionAppService(_lookup, _subscription.Object, _coaches.Object, _stripe.Object);
    }

    [Fact]
    public async Task ScheduleCancelAtPeriodEndAsync_Should_Update_Subscription_When_Active()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByUserIdAsync(coach.Id)).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription { CancelAt = DateTime.UtcNow.AddDays(5) });

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        var result = await _sut.ScheduleCancelAtPeriodEndAsync(coach.Id);

        result.CoachId.Should().Be(coach.Id);
        coach.SubscriptionEndAt.Should().NotBeNull();
        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task ScheduleCancelAtPeriodEndAsync_Should_Return_Without_Stripe_When_Already_Canceled()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");
        coach.SetSubscription(coach.SubscriptionPlan, SubscriptionStatus.Canceled, null);

        _coaches.Setup(r => r.GetByUserIdAsync(coach.Id)).ReturnsAsync(coach);

        var result = await _sut.ScheduleCancelAtPeriodEndAsync(coach.Id);

        result.CoachId.Should().Be(coach.Id);
        _stripe.VerifyNoOtherCalls();
        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }

    [Fact]
    public async Task UndoScheduledCancelAsync_Should_Clear_CancelAt_When_Scheduled()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");
        coach.ScheduleCancellationAt(DateTime.UtcNow.AddDays(5));

        _coaches.Setup(r => r.GetByUserIdAsync(coach.Id)).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription());

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        var result = await _sut.UndoScheduledCancelAsync(coach.Id);

        result.CoachId.Should().Be(coach.Id);
        coach.SubscriptionEndAt.Should().BeNull();
        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task UndoScheduledCancelAsync_Should_Return_Without_Stripe_When_Not_Scheduled()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "sub_123");

        _coaches.Setup(r => r.GetByUserIdAsync(coach.Id)).ReturnsAsync(coach);

        var result = await _sut.UndoScheduledCancelAsync(coach.Id);

        result.CoachId.Should().Be(coach.Id);
        _stripe.VerifyNoOtherCalls();
        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }

    [Fact]
    public async Task Methods_Should_Throw_When_SubscriptionId_Missing()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus_123", "");

        _coaches.Setup(r => r.GetByUserIdAsync(coach.Id)).ReturnsAsync(coach);

        Func<Task> act = () => _sut.ScheduleCancelAtPeriodEndAsync(coach.Id);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}