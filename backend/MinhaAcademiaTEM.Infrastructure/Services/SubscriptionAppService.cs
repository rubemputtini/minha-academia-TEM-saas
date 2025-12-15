using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Admin;
using MinhaAcademiaTEM.Application.DTOs.Subscriptions;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class SubscriptionAppService(
    EntityLookup lookup,
    ISubscriptionSummaryReader subscriptionSummaryReader,
    ICoachRepository coachRepository,
    IStripeClient stripeClient)
    : ISubscriptionAppService
{
    public async Task<SubscriptionSummaryResponse> GetMySubscriptionSummaryAsync(Guid userId)
    {
        var coach = await lookup.GetCoachByUserIdAsync(userId);

        if (string.IsNullOrWhiteSpace(coach.StripeSubscriptionId))
            throw new InvalidOperationException("Assinatura Stripe não associada para este treinador.");

        var summary = await subscriptionSummaryReader.FromSubscriptionIdAsync(coach.StripeSubscriptionId);

        return summary;
    }

    public async Task<UpdateCoachSubscriptionResponse> ScheduleCancelAtPeriodEndAsync(Guid userId)
    {
        var coach = await lookup.GetCoachByUserIdAsync(userId);

        if (string.IsNullOrWhiteSpace(coach.StripeSubscriptionId))
            throw new InvalidOperationException("Assinatura Stripe não associada para este treinador.");

        if (coach.SubscriptionStatus == SubscriptionStatus.Canceled || coach.SubscriptionEndAt.HasValue)
            return MapUpdateCoachSubscription(coach);

        var service = new SubscriptionService(stripeClient);
        var updated = await service.UpdateAsync(
            coach.StripeSubscriptionId,
            new SubscriptionUpdateOptions { CancelAtPeriodEnd = true });

        coach.SetSubscription(coach.SubscriptionPlan, coach.SubscriptionStatus, updated.CancelAt);

        await coachRepository.UpdateAsync(coach);

        return MapUpdateCoachSubscription(coach);
    }

    public async Task<UpdateCoachSubscriptionResponse> UndoScheduledCancelAsync(Guid userId)
    {
        var coach = await lookup.GetCoachByUserIdAsync(userId);

        if (string.IsNullOrWhiteSpace(coach.StripeSubscriptionId))
            throw new InvalidOperationException("Assinatura Stripe não associada para este treinador.");

        if (coach.SubscriptionStatus == SubscriptionStatus.Canceled || !coach.SubscriptionEndAt.HasValue)
            return MapUpdateCoachSubscription(coach);

        var service = new SubscriptionService(stripeClient);

        await service.UpdateAsync(
            coach.StripeSubscriptionId,
            new SubscriptionUpdateOptions
            {
                CancelAtPeriodEnd = false,
                CancelAt = null
            });

        coach.SetSubscription(coach.SubscriptionPlan, coach.SubscriptionStatus, null);

        await coachRepository.UpdateAsync(coach);

        return MapUpdateCoachSubscription(coach);
    }

    private static UpdateCoachSubscriptionResponse MapUpdateCoachSubscription(Coach coach)
    {
        return new UpdateCoachSubscriptionResponse
        {
            CoachId = coach.Id,
            SubscriptionStatus = coach.SubscriptionStatus,
            SubscriptionPlan = coach.SubscriptionPlan,
            SubscriptionEndAt = coach.SubscriptionEndAt,
        };
    }
}