using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Admin;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using Stripe;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class AdminSubscriptionService(
    EntityLookup lookup,
    ICoachRepository coachRepository,
    IStripeClient stripeClient) : IAdminSubscriptionService
{
    public async Task<UpdateCoachSubscriptionResponse> CancelNowAsync(Guid coachId)
    {
        var coach = await lookup.GetCoachAsync(coachId);

        if (string.IsNullOrWhiteSpace(coach.StripeSubscriptionId))
            throw new InvalidOperationException("Assinatura Stripe não vinculada para este treinador.");

        var service = new SubscriptionService(stripeClient);

        await service.CancelAsync(coach.StripeSubscriptionId);
        coach.CancelSubscriptionNow();

        await coachRepository.UpdateAsync(coach);

        var response = new UpdateCoachSubscriptionResponse
        {
            CoachId = coach.Id,
            SubscriptionStatus = coach.SubscriptionStatus,
            SubscriptionPlan = coach.SubscriptionPlan,
            SubscriptionEndAt = coach.SubscriptionEndAt
        };

        return response;
    }
}