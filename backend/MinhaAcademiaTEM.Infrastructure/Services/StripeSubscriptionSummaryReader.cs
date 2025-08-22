using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeSubscriptionSummaryReader(SubscriptionService subscriptionService) : ISubscriptionSummaryReader
{
    public async Task<SubscriptionSummaryResponse> FromSubscriptionIdAsync(string subscriptionId)
    {
        var subscription = await subscriptionService.GetAsync(subscriptionId, new SubscriptionGetOptions
        {
            Expand = new List<string> { "items.data.price", "latest_invoice", "latest_invoice.lines" }
        });

        var items = subscription.Items?.Data
                    ?? throw new InvalidOperationException("Assinatura sem itens configurados.");

        var invoice = subscription.LatestInvoice
                      ?? throw new InvalidOperationException("Assinatura sem fatura associada.");

        var line = invoice.Lines?.Data?.FirstOrDefault()
                   ?? throw new InvalidOperationException("Fatura sem linhas.");

        return new SubscriptionSummaryResponse
        {
            AmountInCents = line.Amount,
            Currency = line.Currency ?? invoice.Currency ?? "BRL",
            NextBillingUtc = items[0].CurrentPeriodEnd,
            PriceId = line.Pricing.PriceDetails.Price ?? items[0].Price.Id,
        };
    }
}