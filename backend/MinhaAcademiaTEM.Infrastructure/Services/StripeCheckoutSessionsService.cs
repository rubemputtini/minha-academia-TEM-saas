using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Extensions;
using Stripe;
using Stripe.Checkout;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class StripeCheckoutSessionsService(
    IOptions<StripeApiConfiguration> stripeOptions,
    ICoachRepository coachRepository,
    EntityLookup lookup,
    ICurrentUserService currentUser) : ICheckoutSessionsService
{
    private readonly StripeApiConfiguration _stripeConfig = stripeOptions.Value;

    public async Task<string> CreateSignupAsync(SubscriptionPlan subscriptionPlan)
    {
        var priceId = _stripeConfig.ResolvePriceIdByPlan(subscriptionPlan);

        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            SuccessUrl = _stripeConfig.SignupSuccessUrl,
            CancelUrl = _stripeConfig.CancelUrl,
            BillingAddressCollection = "required",
            PhoneNumberCollection = new SessionPhoneNumberCollectionOptions { Enabled = true },
            AllowPromotionCodes = true,
            Metadata = new Dictionary<string, string>
            {
                ["app_flow"] = "signup",
                ["app_plan"] = subscriptionPlan.ToString()
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            }
        };

        var request = new RequestOptions { IdempotencyKey = $"checkout_{priceId}_{Guid.NewGuid()}" };
        var service = new SessionService();
        var session = await service.CreateAsync(options, request);

        if (string.IsNullOrWhiteSpace(session.Url))
            throw new InvalidOperationException("Stripe não retornou a URL do Checkout.");

        return session.Url;
    }

    public async Task<string> CreateCoachSubscriptionAsync(SubscriptionPlan subscriptionPlan)
    {
        var priceId = _stripeConfig.ResolvePriceIdByPlan(subscriptionPlan);
        var coach = await lookup.GetCoachByUserIdAsync(currentUser.GetUserId());

        if (!string.IsNullOrWhiteSpace(coach.StripeSubscriptionId))
            throw new InvalidOperationException("Para alterar o plano, use o Billing Portal.");

        var options = new SessionCreateOptions
        {
            Mode = "subscription",
            SuccessUrl = _stripeConfig.SuccessUrl,
            CancelUrl = _stripeConfig.CancelUrl,
            ClientReferenceId = coach.Id.ToString(),
            Customer = string.IsNullOrWhiteSpace(coach.StripeCustomerId) ? null : coach.StripeCustomerId,
            BillingAddressCollection = "required",
            PhoneNumberCollection = new SessionPhoneNumberCollectionOptions { Enabled = true },
            Metadata = new Dictionary<string, string>
            {
                ["app_coach_id"] = coach.Id.ToString(),
                ["app_flow"] = "upgrade",
                ["app_plan"] = subscriptionPlan.ToString()
            },
            LineItems = new List<SessionLineItemOptions>
            {
                new()
                {
                    Price = priceId,
                    Quantity = 1
                }
            }
        };

        var request = new RequestOptions { IdempotencyKey = $"checkout_upgrade_{coach.Id}_{priceId}" };
        var service = new SessionService();
        var session = await service.CreateAsync(options, request);

        if (string.IsNullOrWhiteSpace(session.Url))
            throw new InvalidOperationException("Stripe não retornou a URL do Checkout.");

        return session.Url;
    }

    public async Task ProcessCheckoutCompletedAsync(CheckoutSessionCompletedRequest request)
    {
        if (!Guid.TryParse(request.ClientReferenceId, out var coachId))
            return;

        var coach = await coachRepository.GetByIdAsync(coachId);
        if (coach == null) return;

        var customerId = request.CustomerId ?? coach.StripeCustomerId ?? string.Empty;
        var subscriptionId = !string.IsNullOrWhiteSpace(request.SubscriptionId)
            ? request.SubscriptionId
            : coach.StripeSubscriptionId ?? string.Empty;

        coach.SetStripeData(customerId, subscriptionId);

        var plan = coach.SubscriptionPlan;

        if (request.Metadata.TryGetValue("app_plan", out var appPlan))
        {
            plan = Enum.Parse<SubscriptionPlan>(appPlan, true);
        }

        coach.SetSubscription(plan, coach.SubscriptionStatus, null);

        await coachRepository.UpdateAsync(coach);
    }
}