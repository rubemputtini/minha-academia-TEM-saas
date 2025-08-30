using FluentAssertions;
using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Infrastructure.Services;
using Moq;
using Stripe;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class StripeReferralServiceTests
{
    private readonly Mock<IStripeClient> _stripe = new();
    private readonly IOptions<StripeApiConfiguration> _opts;
    private readonly PromotionCodeService _promoService;
    private readonly StripeReferralService _sut;

    public StripeReferralServiceTests()
    {
        _opts = Options.Create(new StripeApiConfiguration { ReferralCouponId = "COUPON_REF" });
        _promoService = new PromotionCodeService(_stripe.Object);

        _sut = new StripeReferralService(_opts, _promoService, _stripe.Object);
    }

    [Fact]
    public async Task EnsurePromotionCodeForCoachAsync_Should_Return_Existing()
    {
        var coachId = Guid.NewGuid();
        var slug = "rubem";
        var expectedCode = ReferralCode.FromSlug(slug);

        _stripe.Setup(c => c.RequestAsync<StripeList<PromotionCode>>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/promotion_codes")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StripeList<PromotionCode>
            {
                Data = new List<PromotionCode> { new PromotionCode { Code = expectedCode } }
            });

        var result = await _sut.EnsurePromotionCodeForCoachAsync(coachId, slug);

        result.Should().Be(expectedCode);
    }

    [Fact]
    public async Task EnsurePromotionCodeForCoachAsync_Should_Create_When_Not_Exists()
    {
        var coachId = Guid.NewGuid();
        var slug = "coach-xyz";
        var expectedCode = ReferralCode.FromSlug(slug);

        // 1ª chamada: lista vazia
        _stripe.Setup(c => c.RequestAsync<StripeList<PromotionCode>>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/promotion_codes")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new StripeList<PromotionCode> { Data = new List<PromotionCode>() });

        // 2ª chamada: criação
        _stripe.Setup(c => c.RequestAsync<PromotionCode>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains("/v1/promotion_codes")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new PromotionCode { Code = expectedCode });

        var result = await _sut.EnsurePromotionCodeForCoachAsync(coachId, slug);

        result.Should().Be(expectedCode);
    }

    [Fact]
    public async Task ApplyDiscountAsync_Should_Update_Invoice_With_Coupon()
    {
        var invoiceId = "inv_123";

        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains($"/v1/invoices/{invoiceId}")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Invoice());

        await _sut.ApplyDiscountAsync(invoiceId);

        _stripe.Verify(c => c.RequestAsync<Invoice>(
            HttpMethod.Post,
            It.Is<string>(u => u.Contains($"/v1/invoices/{invoiceId}")),
            It.IsAny<BaseOptions>(),
            It.IsAny<RequestOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task MarkReferralCreditGrantedAsync_Should_Set_IdempotencyKey_And_Metadata()
    {
        var invoiceId = "inv_456";

        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Post,
                It.Is<string>(u => u.Contains($"/v1/invoices/{invoiceId}")),
                It.IsAny<BaseOptions>(),
                It.Is<RequestOptions>(r => r.IdempotencyKey == $"invoice:mark_referral_granted:{invoiceId}"),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Invoice());

        await _sut.MarkReferralCreditGrantedAsync(invoiceId);

        _stripe.Verify(c => c.RequestAsync<Invoice>(
            HttpMethod.Post,
            It.Is<string>(u => u.Contains($"/v1/invoices/{invoiceId}")),
            It.IsAny<BaseOptions>(),
            It.Is<RequestOptions>(r => r.IdempotencyKey == $"invoice:mark_referral_granted:{invoiceId}"),
            It.IsAny<CancellationToken>()), Times.Once);
    }
}