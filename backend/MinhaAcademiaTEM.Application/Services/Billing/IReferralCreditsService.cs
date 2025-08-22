namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface IReferralCreditsService
{
    Task AddCreditForReferrerAsync(string promotionCodeId);
    Task ApplyMonthlyDiscountIfEligibleAsync(string subscriptionId, string invoiceId, DateTime periodStartUtc);
}