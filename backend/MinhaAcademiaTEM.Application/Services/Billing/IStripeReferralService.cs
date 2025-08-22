namespace MinhaAcademiaTEM.Application.Services.Billing;

public interface IStripeReferralService
{
    Task<string> EnsurePromotionCodeForCoachAsync(Guid coachId, string slug);
    Task ApplyDiscountAsync(string invoiceId);
    Task MarkReferralCreditGrantedAsync(string invoiceId);
}