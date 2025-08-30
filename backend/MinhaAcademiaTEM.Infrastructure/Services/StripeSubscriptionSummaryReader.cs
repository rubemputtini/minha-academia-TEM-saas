using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeSubscriptionSummaryReader(SubscriptionService subscriptionService) : ISubscriptionSummaryReader
{
    public async Task<SubscriptionSummaryResponse> FromSubscriptionIdAsync(string subscriptionId)
    {
        if (string.IsNullOrWhiteSpace(subscriptionId))
            throw new ArgumentException("O subscriptionId é obrigatório.", nameof(subscriptionId));

        var subscription = await subscriptionService.GetAsync(subscriptionId, new SubscriptionGetOptions
        {
            Expand = new List<string>
                { "items.data.price", "latest_invoice", "latest_invoice.lines" }
        });

        var items = subscription.Items?.Data;
        if (items == null || items.Count == 0)
            throw new InvalidOperationException("Assinatura sem itens configurados.");

        var invoice = subscription.LatestInvoice
                      ?? throw new InvalidOperationException("Assinatura sem fatura associada.");

        var line = invoice.Lines?.Data?.FirstOrDefault()
                   ?? throw new InvalidOperationException("Fatura sem linhas.");

        var priceId =
            line.Pricing?.PriceDetails?.Price
            ?? items[0].Price?.Id;

        if (string.IsNullOrWhiteSpace(priceId))
            throw new InvalidOperationException("Não foi possível identificar o PriceId da assinatura.");

        var amount = line.Amount;
        var currency = line.Currency ?? invoice.Currency ?? "BRL";
        var nextBilling = items[0].CurrentPeriodEnd;

        return new SubscriptionSummaryResponse
        {
            AmountInCents = amount,
            Currency = currency,
            NextBillingUtc = nextBilling,
            PriceId = priceId
        };
    }
}