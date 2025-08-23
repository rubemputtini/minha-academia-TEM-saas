using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Subscriptions;

public interface IPlanCapabilityResolver
{
    bool HasCapability(SubscriptionPlan subscriptionPlan, Capability capability);
}