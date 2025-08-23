using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.Subscriptions;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Exceptions;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public class PlanRulesService(EntityLookup lookup, IPlanCapabilityResolver resolver) : IPlanRulesService
{
    public async Task EnsureCapabilityAsync(Guid coachId, Capability capability)
    {
        var coach = await lookup.GetCoachAsync(coachId);

        if (!coach.HasAccess)
            throw new ForbiddenException("Sua assinatura está inativa. Regularize o pagamento para reativar o acesso.");

        if (!resolver.HasCapability(coach.SubscriptionPlan, capability))
            throw new ForbiddenException($"O plano {coach.SubscriptionPlan} não permite esta ação.");
    }
}