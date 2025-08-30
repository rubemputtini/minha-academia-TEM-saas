using FluentAssertions;
using Moq;
using Stripe;
using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Infrastructure.Services;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class SubscriptionSyncServiceTests
{
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<IStripeClient> _stripe = new();

    private readonly EntityLookup _lookup;
    private readonly IOptions<StripeApiConfiguration> _opts;
    private readonly SubscriptionSyncService _sut;

    public SubscriptionSyncServiceTests()
    {
        _lookup = new EntityLookup(_users.Object, _coaches.Object, _gyms.Object);

        _opts = Options.Create(new StripeApiConfiguration
        {
            PriceIds = new Dictionary<string, string>
            {
                ["Basic"] = "price_basic",
                ["Unlimited"] = "price_unlimited"
            }
        });
        
        _sut = new SubscriptionSyncService(_lookup, _opts, _coaches.Object, _stripe.Object);
    }

    [Fact]
    public async Task UpdateAsync_When_Status_Is_Canceled_Should_SetCanceled_And_Persist()
    {
        var coach = TestData.Coach();
        var req = new UpdateSubscriptionRequest
        {
            CustomerId = "cus_123",
            SubscriptionId = "sub_123",
            StripeStatus = "canceled",
            PriceId = "price_basic"
        };

        _coaches.Setup(r => r.GetByStripeCustomerIdAsync("cus_123")).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription());

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        await _sut.UpdateAsync(req);

        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Canceled);
        coach.StripeSubscriptionId.Should().BeNull();
        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_When_Active_And_No_Cancel_Scheduled_Should_Set_Active_With_Null_EndAt()
    {
        var coach = TestData.Coach();
        var req = new UpdateSubscriptionRequest
        {
            CustomerId = "cus_123",
            SubscriptionId = "sub_123",
            StripeStatus = "active",
            PriceId = "price_basic"
        };

        _coaches.Setup(r => r.GetByStripeCustomerIdAsync("cus_123")).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription
            {
                CancelAtPeriodEnd = false,
                CancelAt = null
            });

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        await _sut.UpdateAsync(req);

        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Active);
        coach.SubscriptionEndAt.Should().BeNull();
        coach.StripeCustomerId.Should().Be("cus_123");
        coach.StripeSubscriptionId.Should().Be("sub_123");
        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_When_Active_And_CancelAtPeriodEnd_Should_Set_EndAt()
    {
        var coach = TestData.Coach();
        var req = new UpdateSubscriptionRequest
        {
            CustomerId = "cus_123",
            SubscriptionId = "sub_123",
            StripeStatus = "active",
            PriceId = "price_basic"
        };

        var cancelAt = DateTime.UtcNow.AddDays(10);

        _coaches.Setup(r => r.GetByStripeCustomerIdAsync("cus_123")).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription
            {
                CancelAtPeriodEnd = true,
                CancelAt = cancelAt
            });

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        await _sut.UpdateAsync(req);

        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Active);
        coach.SubscriptionEndAt.Should().Be(cancelAt); 
        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_When_Active_And_Only_CancelAt_Should_Set_EndAt()
    {
        var coach = TestData.Coach();
        var req = new UpdateSubscriptionRequest
        {
            CustomerId = "cus_123",
            SubscriptionId = "sub_123",
            StripeStatus = "active",
            PriceId = "price_basic"
        };

        var cancelAt = DateTime.UtcNow.AddDays(3);

        _coaches.Setup(r => r.GetByStripeCustomerIdAsync("cus_123")).ReturnsAsync(coach);

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Subscription
            {
                CancelAtPeriodEnd = false,
                CancelAt = cancelAt
            });

        _coaches.Setup(r => r.UpdateAsync(coach)).Returns(Task.CompletedTask);

        await _sut.UpdateAsync(req);

        coach.SubscriptionStatus.Should().Be(SubscriptionStatus.Active);
        coach.SubscriptionEndAt.Should().Be(cancelAt);
        _coaches.Verify(r => r.UpdateAsync(coach), Times.Once);
    }
}