using FluentAssertions;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Infrastructure.Extensions;

namespace MinhaAcademiaTEM.UnitTests.Infrastructure.Extensions;

public class StripeConfigExtensionsTests
{
    private static StripeApiConfiguration Cfg() => new()
    {
        PriceIds = new Dictionary<string, string>
        {
            [nameof(SubscriptionPlan.Basic)] = "price_basic",
            [nameof(SubscriptionPlan.Unlimited)] = "price_unlimited"
        }
    };

    [Fact]
    public void ResolvePriceIdByPlan_Should_Return_Basic()
    {
        var cfg = Cfg();
        var id = cfg.ResolvePriceIdByPlan(SubscriptionPlan.Basic);

        id.Should().Be("price_basic");
    }

    [Fact]
    public void ResolvePriceIdByPlan_Should_Throw_When_Not_Configured()
    {
        var cfg = new StripeApiConfiguration { PriceIds = new Dictionary<string, string>() };
        Action act = () => cfg.ResolvePriceIdByPlan(SubscriptionPlan.Basic);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ResolvePriceIdByPlan_Should_Throw_When_Empty_String()
    {
        var cfg = new StripeApiConfiguration
        {
            PriceIds = new Dictionary<string, string>
            {
                [nameof(SubscriptionPlan.Basic)] = "  "
            }
        };
        Action act = () => cfg.ResolvePriceIdByPlan(SubscriptionPlan.Basic);

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ResolvePlanByPriceId_Should_Return_Basic()
    {
        var cfg = Cfg();
        var plan = cfg.ResolvePlanByPriceId("price_basic");

        plan.Should().Be(SubscriptionPlan.Basic);
    }

    [Fact]
    public void ResolvePlanByPriceId_Should_Return_Unlimited()
    {
        var cfg = Cfg();
        var plan = cfg.ResolvePlanByPriceId("price_unlimited");

        plan.Should().Be(SubscriptionPlan.Unlimited);
    }

    [Fact]
    public void ResolvePlanByPriceId_Should_Be_Case_Sensitive_And_Throw_When_Different_Case()
    {
        var cfg = Cfg();
        Action act = () => cfg.ResolvePlanByPriceId("PRICE_BASIC");

        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void ResolvePlanByPriceId_Should_Throw_When_Unknown()
    {
        var cfg = Cfg();
        Action act = () => cfg.ResolvePlanByPriceId("price_other");

        act.Should().Throw<InvalidOperationException>();
    }
}