using Microsoft.Extensions.Options;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Configuration;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Extensions;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class SubscriptionSyncService(
    EntityLookup lookup,
    IOptions<StripeApiConfiguration> stripeOptions,
    ICoachRepository coachRepository)
    : ISubscriptionSyncService
{
    private readonly StripeApiConfiguration _stripeConfig = stripeOptions.Value;

    public async Task UpdateAsync(UpdateSubscriptionRequest request)
    {
        var coach = await lookup.GetCoachByStripeCustomerIdAsync(request.CustomerId);

        coach.SetStripeData(request.CustomerId, request.SubscriptionId);

        var subscriptionService = new SubscriptionService();
        var subscription = await subscriptionService.GetAsync(request.SubscriptionId);

        var plan = _stripeConfig.ResolvePlanByPriceId(request.PriceId);
        var status = MapStripeStatus(request.StripeStatus);

        if (status == SubscriptionStatus.Canceled)
        {
            coach.SetCanceled();
            await coachRepository.UpdateAsync(coach);

            return;
        }

        if (subscription.CancelAtPeriodEnd || subscription.CancelAt.HasValue)
        {
            coach.SetSubscription(plan, status, subscription.CancelAt);
        }
        else
        {
            coach.SetSubscription(plan, status, null);
        }

        await coachRepository.UpdateAsync(coach);
    }

    private static SubscriptionStatus MapStripeStatus(string s) => s switch
    {
        "active" => SubscriptionStatus.Active,
        "trialing" => SubscriptionStatus.Trial,
        "past_due" => SubscriptionStatus.PastDue,
        "unpaid" => SubscriptionStatus.PastDue,
        "incomplete" => SubscriptionStatus.PastDue,
        "incomplete_expired" => SubscriptionStatus.Canceled,
        "canceled" => SubscriptionStatus.Canceled,
        _ => SubscriptionStatus.PastDue
    };
}