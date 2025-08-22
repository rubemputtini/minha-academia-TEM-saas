using MinhaAcademiaTEM.Application.Services.Billing;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class InvoiceDiscountReader() : IInvoiceDiscountReader
{
    public async Task<IReadOnlyList<string>> GetPromotionCodeIdsAsync(string invoiceId)
    {
        var options = new InvoiceGetOptions
        {
            Expand = ["discounts"]
        };

        var invoiceService = new InvoiceService();
        var invoice = await invoiceService.GetAsync(invoiceId, options);

        if (invoice.Discounts == null || invoice.Discounts.Count == 0)
            return [];

        return invoice.Discounts
            .Select(d => d.PromotionCodeId)
            .Where(id => !string.IsNullOrWhiteSpace(id))
            .ToList();
    }
}