using MinhaAcademiaTEM.Application.Services.Billing;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class InvoiceDiscountReader(IStripeClient stripeClient) : IInvoiceDiscountReader
{
    public async Task<IReadOnlyList<string>> GetPromotionCodeIdsAsync(string invoiceId)
    {
        if (string.IsNullOrWhiteSpace(invoiceId))
            throw new ArgumentException("O invoiceId é obrigatório.", nameof(invoiceId));

        var options = new InvoiceGetOptions
        {
            Expand = ["discounts"]
        };

        var invoiceService = new InvoiceService(stripeClient);
        var invoice = await invoiceService.GetAsync(invoiceId, options);

        var discounts = invoice?.Discounts;

        if (discounts == null || discounts.Count == 0)
            return [];

        return discounts
            .Select(d => d.PromotionCodeId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToList();
    }
}