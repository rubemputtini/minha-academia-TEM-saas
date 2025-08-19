using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Infrastructure.Extensions;

public static class StripeConfigExtensions
{
    public static SubscriptionPlan ResolvePlanByPriceId(this StripeApiConfiguration config, string priceId)
    {
        if (config.PriceIds.TryGetValue(nameof(SubscriptionPlan.Basic), out var basicId) &&
            !string.IsNullOrWhiteSpace(basicId) &&
            string.Equals(priceId, basicId, StringComparison.Ordinal))
        {
            return SubscriptionPlan.Basic;
        }

        if (config.PriceIds.TryGetValue(nameof(SubscriptionPlan.Unlimited), out var unlimitedId) &&
            !string.IsNullOrWhiteSpace(unlimitedId) &&
            string.Equals(priceId, unlimitedId, StringComparison.Ordinal))
        {
            return SubscriptionPlan.Unlimited;
        }

        throw new InvalidOperationException($"Stripe PriceId não configurado: '{priceId}'.");
    }

    public static string ResolvePriceIdByPlan(this StripeApiConfiguration config, SubscriptionPlan subscriptionPlan)
    {
        if (config.PriceIds.TryGetValue(subscriptionPlan.ToString(), out var priceId) &&
            !string.IsNullOrWhiteSpace(priceId))
            return priceId;

        throw new InvalidOperationException($"Stripe PriceId não configurado para o plano '{subscriptionPlan}'.");
    }
}