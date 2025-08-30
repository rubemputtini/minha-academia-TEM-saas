using FluentAssertions;
using Moq;
using Stripe;
using MinhaAcademiaTEM.Infrastructure.Services;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class StripeSubscriptionSummaryReaderTests
{
    private readonly Mock<IStripeClient> _stripe = new();

    private StripeSubscriptionSummaryReader CreateSut()
    {
        var svc = new SubscriptionService(_stripe.Object);

        return new StripeSubscriptionSummaryReader(svc);
    }

    [Fact]
    public async Task FromSubscriptionIdAsync_Should_Map_All_Fields()
    {
        var next = DateTime.UtcNow.AddDays(10);

        var sub = new Subscription
        {
            Id = "sub_123",
            Items = new StripeList<SubscriptionItem>
            {
                Data = new List<SubscriptionItem>
                {
                    new SubscriptionItem
                    {
                        Price = new Price { Id = "price_basic" },
                        CurrentPeriodEnd = next
                    }
                }
            },
            LatestInvoice = new Invoice
            {
                Currency = "usd",
                Lines = new StripeList<InvoiceLineItem>
                {
                    Data = new List<InvoiceLineItem>
                    {
                        new InvoiceLineItem
                        {
                            Amount = 1299,
                            Currency = "usd"
                        }
                    }
                }
            }
        };

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/sub_123")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sub);

        var sut = CreateSut();
        var res = await sut.FromSubscriptionIdAsync("sub_123");

        res.AmountInCents.Should().Be(1299);
        res.Currency.Should().Be("usd");
        res.NextBillingUtc.Should().Be(next);
        res.PriceId.Should().Be("price_basic");
    }

    [Fact]
    public async Task FromSubscriptionIdAsync_Should_Throw_When_No_Items()
    {
        var sub = new Subscription
        {
            Id = "sub_noitems", Items = null,
            LatestInvoice = new Invoice
                { Lines = new StripeList<InvoiceLineItem> { Data = new() { new InvoiceLineItem { Amount = 100 } } } }
        };

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/sub_noitems")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sub);

        var sut = CreateSut();
        var act = () => sut.FromSubscriptionIdAsync("sub_noitems");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task FromSubscriptionIdAsync_Should_Throw_When_No_Invoice()
    {
        var sub = new Subscription
        {
            Id = "sub_noinv",
            Items = new StripeList<SubscriptionItem>
            {
                Data = new()
                    { new SubscriptionItem { Price = new Price { Id = "p" }, CurrentPeriodEnd = DateTime.UtcNow } }
            },
            LatestInvoice = null
        };

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/sub_noinv")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sub);

        var sut = CreateSut();
        var act = () => sut.FromSubscriptionIdAsync("sub_noinv");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task FromSubscriptionIdAsync_Should_Throw_When_No_Lines()
    {
        var sub = new Subscription
        {
            Id = "sub_nolines",
            Items = new StripeList<SubscriptionItem>
            {
                Data = new()
                    { new SubscriptionItem { Price = new Price { Id = "p" }, CurrentPeriodEnd = DateTime.UtcNow } }
            },
            LatestInvoice = new Invoice
                { Lines = new StripeList<InvoiceLineItem> { Data = new List<InvoiceLineItem>() } }
        };

        _stripe.Setup(c => c.RequestAsync<Subscription>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/subscriptions/sub_nolines")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(sub);

        var sut = CreateSut();
        var act = () => sut.FromSubscriptionIdAsync("sub_nolines");

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}