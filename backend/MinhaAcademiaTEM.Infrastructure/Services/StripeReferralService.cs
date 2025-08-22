using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Configuration;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeReferralService(
    IOptions<StripeApiConfiguration> stripeOptions,
    PromotionCodeService promotionCodeService) : IStripeReferralService
{
    private readonly StripeApiConfiguration _stripeConfig = stripeOptions.Value;

    public async Task<string> EnsurePromotionCodeForCoachAsync(Guid coachId, string slug)
    {
        var code = ReferralCode.FromSlug(slug);

        var list = await promotionCodeService.ListAsync(new PromotionCodeListOptions
        {
            Code = code,
            Active = true,
            Limit = 1,
        });

        var existing = list.Data.FirstOrDefault();
        if (existing != null) return existing.Code;

        var create = new PromotionCodeCreateOptions
        {
            Coupon = _stripeConfig.ReferralCouponId,
            Code = code,
            Restrictions = new PromotionCodeRestrictionsOptions
            {
                FirstTimeTransaction = true
            },
            Metadata = new Dictionary<string, string>
            {
                ["coachId"] = coachId.ToString(),
                ["slug"] = slug,
            }
        };

        var created = await promotionCodeService.CreateAsync(create);

        return created.Code;
    }

    public async Task ApplyDiscountAsync(string invoiceId)
    {
        var invoiceService = new InvoiceService();

        var update = new InvoiceUpdateOptions
        {
            Discounts = new List<InvoiceDiscountOptions>
            {
                new() { Coupon = _stripeConfig.ReferralCouponId }
            }
        };

        await invoiceService.UpdateAsync(invoiceId, update);
    }
}