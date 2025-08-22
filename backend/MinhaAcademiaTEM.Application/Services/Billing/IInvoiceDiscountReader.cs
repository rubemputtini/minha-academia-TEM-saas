namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface IInvoiceDiscountReader
{
    Task<IReadOnlyList<string>> GetPromotionCodeIdsAsync(string invoiceId);
}