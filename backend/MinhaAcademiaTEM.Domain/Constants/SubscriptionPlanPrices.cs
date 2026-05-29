using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Constants;

public static class SubscriptionPlanPrices
{
    public const decimal BasicBrl = 24.90m;
    public const decimal UnlimitedBrl = 39.90m;

    public static decimal GetMonthlyBrl(SubscriptionPlan plan) => plan switch
    {
        SubscriptionPlan.Basic => BasicBrl,
        SubscriptionPlan.Unlimited => UnlimitedBrl,
        _ => 0m
    };
}
