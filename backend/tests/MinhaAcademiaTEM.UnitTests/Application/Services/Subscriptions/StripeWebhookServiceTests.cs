using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Infrastructure.Services;
using Moq;
using Stripe;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Subscriptions;

public class StripeWebhookServiceTests
{
    private readonly Mock<ICheckoutSessionsService> _checkout = new();
    private readonly Mock<ISubscriptionSyncService> _sync = new();
    private readonly Mock<IReferralCreditsService> _referralCredits = new();
    private readonly Mock<IInvoiceDiscountReader> _discountReader = new();
    private readonly Mock<IStripeReferralService> _stripeReferral = new();
    private readonly Mock<IStripeClient> _stripe = new();

    private readonly IOptions<StripeApiConfiguration> _opts;

    public StripeWebhookServiceTests()
    {
        _opts = Options.Create(new StripeApiConfiguration { WebhookSecret = "whsec_test" });
    }

    private StripeWebhookService CreateSut() =>
        new(
            _opts,
            _checkout.Object,
            _sync.Object,
            _referralCredits.Object,
            _discountReader.Object,
            _stripeReferral.Object,
            _stripe.Object);

    // Assina como o Stripe: v1 = HMACSHA256("{ts}.{payload}")
    private static string Sign(string payload, string secret)
    {
        var ts = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var toSign = $"{ts}.{payload}";
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var sig = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(toSign))).ToLowerInvariant();
        
        return $"t={ts},v1={sig}";
    }

    // ---------- checkout.session.completed ----------

    [Fact]
    public async Task HandleAsync_CheckoutSessionCompleted_Should_Call_ProcessCheckout()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_checkout___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "cs_test",
                  "object": "checkout.session",
                  "mode": "subscription",
                  "client_reference_id": "coach-guid",
                  "customer": "cus_123",
                  "subscription": "sub_123",
                  "metadata": { "app_plan": "Basic" }
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "checkout.session.completed"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _checkout.Verify(c => c.ProcessCheckoutCompletedAsync(
            It.Is<CheckoutSessionCompletedRequest>(r =>
                r.Mode == "subscription" &&
                r.ClientReferenceId == "coach-guid" &&
                r.CustomerId == "cus_123" &&
                r.SubscriptionId == "sub_123" &&
                r.Metadata!["app_plan"] == "Basic"
            )), Times.Once);
    }

    // ---------- customer.subscription.updated ----------

    [Fact]
    public async Task HandleAsync_SubscriptionUpdated_Should_Map_And_Call_Sync()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_subupd___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "sub_1",
                  "object": "subscription",
                  "customer": "cus_1",
                  "status": "active",
                  "items": {
                    "object": "list",
                    "data": [
                      {
                        "id": "si_1",
                        "object": "subscription_item",
                        "price": {
                          "id": "price_basic",
                          "object": "price"
                        }
                      }
                    ]
                  }
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "customer.subscription.updated"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _sync.Verify(s => s.UpdateAsync(
            It.Is<UpdateSubscriptionRequest>(r =>
                r.CustomerId == "cus_1" &&
                r.SubscriptionId == "sub_1" &&
                r.PriceId == "price_basic" &&
                r.StripeStatus == "active"
            )), Times.Once);
    }

    // ---------- invoice.created (non-draft = no-op) ----------

    [Fact]
    public async Task HandleAsync_InvoiceCreated_NonDraft_Should_NoOp()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_invcreated_open___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "in_open",
                  "object": "invoice",
                  "status": "open"
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "invoice.created"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _referralCredits.Verify(r => r.ApplyMonthlyDiscountIfEligibleAsync(
            It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>()), Times.Never);
    }

    // ---------- invoice.paid (billing_reason != subscription_create) = no-op ----------

    [Fact]
    public async Task HandleAsync_InvoicePaid_NonSubscriptionCreate_Should_NoOp()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_invpaid_other___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "in_other",
                  "object": "invoice",
                  "billing_reason": "manual"
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "invoice.paid"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _discountReader.Verify(d => d.GetPromotionCodeIdsAsync(It.IsAny<string>()), Times.Never);
        _referralCredits.Verify(r => r.AddCreditForReferrerAsync(It.IsAny<string>()), Times.Never);
        _stripeReferral.Verify(s => s.MarkReferralCreditGrantedAsync(It.IsAny<string>()), Times.Never);
    }

    // ---------- invoice.paid (subscription_create) com promos ----------

    [Fact]
    public async Task HandleAsync_InvoicePaid_SubscriptionCreate_With_Promos_Should_Grant_And_Mark()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_invpaid_promos___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "in_paid",
                  "object": "invoice",
                  "billing_reason": "subscription_create"
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "invoice.paid"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/invoices/in_paid")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Invoice { Id = "in_paid" });

        _discountReader.Setup(d => d.GetPromotionCodeIdsAsync("in_paid"))
            .ReturnsAsync(new List<string> { "promo_1", "promo_2" });

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _referralCredits.Verify(r => r.AddCreditForReferrerAsync("promo_1"), Times.Once);
        _referralCredits.Verify(r => r.AddCreditForReferrerAsync("promo_2"), Times.Once);
        _stripeReferral.Verify(s => s.MarkReferralCreditGrantedAsync("in_paid"), Times.Once);
    }

    // ---------- invoice.paid (subscription_create) já marcado = no-op ----------

    [Fact]
    public async Task HandleAsync_InvoicePaid_AlreadyGranted_Should_NoOp()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_invpaid_granted___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "in_granted",
                  "object": "invoice",
                  "billing_reason": "subscription_create"
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "invoice.paid"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/invoices/in_granted")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Invoice
            {
                Id = "in_granted",
                Metadata = new Dictionary<string, string> { ["referral_credit_granted"] = "true" }
            });

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _discountReader.Verify(d => d.GetPromotionCodeIdsAsync(It.IsAny<string>()), Times.Never);
        _referralCredits.Verify(r => r.AddCreditForReferrerAsync(It.IsAny<string>()), Times.Never);
        _stripeReferral.Verify(s => s.MarkReferralCreditGrantedAsync(It.IsAny<string>()), Times.Never);
    }

    // ---------- invoice.paid (subscription_create) sem promos = no-op ----------

    [Fact]
    public async Task HandleAsync_InvoicePaid_NoPromos_Should_NoOp()
    {
        var created = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        var evt = Guid.NewGuid().ToString("N");

        var payload = """
            {
              "id": "evt_test_invpaid_nopromos___EVT__",
              "object": "event",
              "api_version": "__API__",
              "created": __CREATED__,
              "data": {
                "object": {
                  "id": "in_nopromos",
                  "object": "invoice",
                  "billing_reason": "subscription_create"
                }
              },
              "livemode": false,
              "pending_webhooks": 1,
              "request": { "id": null, "idempotency_key": null },
              "type": "invoice.paid"
            }
            """
            .Replace("__EVT__", evt)
            .Replace("__CREATED__", created.ToString())
            .Replace("__API__", StripeConfiguration.ApiVersion);

        _stripe.Setup(c => c.RequestAsync<Invoice>(
                HttpMethod.Get,
                It.Is<string>(u => u.Contains("/v1/invoices/in_nopromos")),
                It.IsAny<BaseOptions>(),
                It.IsAny<RequestOptions>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Invoice { Id = "in_nopromos" });

        _discountReader.Setup(d => d.GetPromotionCodeIdsAsync("in_nopromos"))
            .ReturnsAsync(new List<string>());

        var header = Sign(payload, _opts.Value.WebhookSecret);
        var sut = CreateSut();

        await sut.HandleAsync(payload, header);

        _referralCredits.Verify(r => r.AddCreditForReferrerAsync(It.IsAny<string>()), Times.Never);
        _stripeReferral.Verify(s => s.MarkReferralCreditGrantedAsync(It.IsAny<string>()), Times.Never);
    }
}