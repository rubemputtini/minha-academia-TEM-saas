using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Services;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;
using Moq;
using Stripe;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class ReferralCreditsServiceTests
{
    private readonly Mock<IReferralAccountRepository> _accounts = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IStripeReferralService> _stripeReferral = new();
    private readonly Mock<IStripeClient> _stripe = new();

    private readonly PromotionCodeService _promoService;
    private readonly IOptions<StripeApiConfiguration> _opts;
    private readonly ReferralCreditsService _sut;

    public ReferralCreditsServiceTests()
    {
        _opts = Options.Create(new StripeApiConfiguration { ReferralCouponId = "COUPON_REF" });
        _promoService = new PromotionCodeService(_stripe.Object);

        _sut = new ReferralCreditsService(
            _accounts.Object,
            _coaches.Object,
            _stripeReferral.Object,
            _promoService,
            _opts
        );
    }

    [Fact]
    public async Task AddCreditForReferrerAsync_Should_Create_When_Account_Not_Exists_And_Coupon_Matches()
    {
        var coachId = Guid.NewGuid();
        var promoId = "promo_123";

        _stripe.Setup(c => c.RequestAsync<PromotionCode>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains($"/v1/promotion_codes/{promoId}")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PromotionCode
            {
                Id = promoId,
                Coupon = new Coupon { Id = "COUPON_REF" },
                Metadata = new Dictionary<string, string> { ["coachId"] = coachId.ToString() }
            });

        _accounts.Setup(a => a.GetByCoachIdAsync(coachId)).ReturnsAsync((ReferralAccount?)null);
        _accounts.Setup(a => a.AddAsync(It.IsAny<ReferralAccount>())).Returns(Task.CompletedTask);

        await _sut.AddCreditForReferrerAsync(promoId);

        _accounts.Verify(a => a.AddAsync(It.Is<ReferralAccount>(r => r.CoachId == coachId)), Times.Once);
    }

    [Fact]
    public async Task AddCreditForReferrerAsync_Should_NoOp_When_Coupon_Different()
    {
        var promoId = "promo_999";

        _stripe.Setup(c => c.RequestAsync<PromotionCode>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains($"/v1/promotion_codes/{promoId}")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PromotionCode
            {
                Id = promoId,
                Coupon = new Coupon { Id = "OTHER" },
                Metadata = new Dictionary<string, string>()
            });

        await _sut.AddCreditForReferrerAsync(promoId);

        _accounts.Verify(a => a.AddAsync(It.IsAny<ReferralAccount>()), Times.Never);
        _accounts.Verify(a => a.UpdateAsync(It.IsAny<ReferralAccount>()), Times.Never);
    }

    [Fact]
    public async Task ApplyMonthlyDiscountIfEligibleAsync_Should_Apply_And_Update()
    {
        var coach = TestData.Coach();
        coach.SetStripeData("cus", "sub_ok");

        var account = TestData.ReferralAccount(coach.Id);
        account.AddCredit();

        var period = DateTime.UtcNow;

        _coaches.Setup(r => r.GetByStripeSubscriptionIdAsync("sub_ok")).ReturnsAsync(coach);
        _accounts.Setup(a => a.GetByCoachIdAsync(coach.Id)).ReturnsAsync(account);
        _stripeReferral.Setup(s => s.ApplyDiscountAsync("inv_ok")).Returns(Task.CompletedTask);
        _accounts.Setup(a => a.UpdateAsync(account)).Returns(Task.CompletedTask);

        await _sut.ApplyMonthlyDiscountIfEligibleAsync("sub_ok", "inv_ok", period);

        _stripeReferral.Verify(s => s.ApplyDiscountAsync("inv_ok"), Times.Once);
        _accounts.Verify(a => a.UpdateAsync(account), Times.Once);
    }
}