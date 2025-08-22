using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class ReferralCreditsService(
    IReferralAccountRepository accountRepository,
    ICoachRepository coachRepository,
    IStripeReferralService stripeReferralService,
    PromotionCodeService promotionCodeService,
    IOptions<StripeApiConfiguration> stripeOptions) : IReferralCreditsService
{
    private readonly StripeApiConfiguration _stripeConfig = stripeOptions.Value;

    public async Task AddCreditForReferrerAsync(string promotionCodeId)
    {
        var code = await promotionCodeService.GetAsync(promotionCodeId);

        if (code?.Coupon?.Id != _stripeConfig.ReferralCouponId) return;

        if (code.Metadata == null || !code.Metadata.TryGetValue("coachId", out var coachIdStr)) return;
        if (!Guid.TryParse(coachIdStr, out var coachId)) return;

        var account = await accountRepository.GetByCoachIdAsync(coachId);

        if (account == null)
        {
            account = new ReferralAccount(coachId);
            account.AddCredit();
            await accountRepository.AddAsync(account);
        }
        else
        {
            account.AddCredit();
            await accountRepository.UpdateAsync(account);
        }
    }

    public async Task ApplyMonthlyDiscountIfEligibleAsync(string subscriptionId, string invoiceId,
        DateTime periodStartUtc)
    {
        var coach = await coachRepository.GetByStripeSubscriptionIdAsync(subscriptionId);
        if (coach == null) return;

        var account = await accountRepository.GetByCoachIdAsync(coach.Id);
        if (account == null) return;

        if (!account.CanApplyForPeriod(periodStartUtc)) return;

        await stripeReferralService.ApplyDiscountAsync(invoiceId);
        account.MarkApplied(periodStartUtc);

        await accountRepository.UpdateAsync(account);
    }
}