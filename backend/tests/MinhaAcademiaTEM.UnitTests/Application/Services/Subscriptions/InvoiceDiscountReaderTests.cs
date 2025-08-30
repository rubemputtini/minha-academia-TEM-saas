using FluentAssertions;
using Moq;
using Stripe;
using MinhaAcademiaTEM.Infrastructure.Services;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class InvoiceDiscountReaderTests
{
    private readonly Mock<IStripeClient> _stripe = new();

    private InvoiceDiscountReader CreateSut() => new(_stripe.Object);

    [Fact]
    public async Task GetPromotionCodeIdsAsync_Should_Throw_When_InvoiceId_Empty()
    {
        var sut = CreateSut();
        var act = () => sut.GetPromotionCodeIdsAsync("");
        await act.Should().ThrowAsync<ArgumentException>();
    }

    [Fact]
    public async Task GetPromotionCodeIdsAsync_Should_Return_Empty_When_No_Discounts()
    {
        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Get,
                It.IsAny<string>(),
                It.Is<BaseOptions>(o =>
                    o is InvoiceGetOptions &&
                    ((InvoiceGetOptions)o).Expand != null &&
                    ((InvoiceGetOptions)o).Expand.Contains("discounts")
                ),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Invoice { Id = "in_empty", Discounts = null });

        var sut = CreateSut();
        var result = await sut.GetPromotionCodeIdsAsync("in_empty");

        result.Should().BeEmpty();

        _stripe.Verify(c => c.RequestAsync<Invoice>(
                HttpMethod.Get,
                It.IsAny<string>(),
                It.Is<BaseOptions>(o =>
                    o is InvoiceGetOptions &&
                    ((InvoiceGetOptions)o).Expand != null &&
                    ((InvoiceGetOptions)o).Expand.Contains("discounts")
                ),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task GetPromotionCodeIdsAsync_Should_Filter_Only_NonEmpty_PromotionCodeIds()
    {
        var invoice = new Invoice
        {
            Id = "in_ok",
            Discounts = new List<Discount>
            {
                new() { PromotionCodeId = "promo_1" },
                new() { PromotionCodeId = null },
                new() { PromotionCodeId = "  " },
                new() { PromotionCodeId = "promo_2" }
            }
        };

        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Get,
                It.IsAny<string>(),
                It.Is<BaseOptions>(o =>
                    o is InvoiceGetOptions &&
                    ((InvoiceGetOptions)o).Expand != null &&
                    ((InvoiceGetOptions)o).Expand.Contains("discounts")
                ),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(invoice);

        var sut = CreateSut();
        var result = await sut.GetPromotionCodeIdsAsync("in_ok");

        result.Should().BeEquivalentTo(new[] { "promo_1", "promo_2" }, o => o.WithoutStrictOrdering());
    }
}