using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Subscriptions;

public class PlanCapabilityResolver : IPlanCapabilityResolver
{
    private static readonly IReadOnlyDictionary<SubscriptionPlan, HashSet<Capability>> Map = Build();

    public bool HasCapability(SubscriptionPlan subscriptionPlan, Capability capability) =>
        Map.TryGetValue(subscriptionPlan, out var capabilities) && capabilities.Contains(capability);

    private static IReadOnlyDictionary<SubscriptionPlan, HashSet<Capability>> Build()
    {
        var trial = new HashSet<Capability>();

        var basic = new HashSet<Capability>(trial)
        {
            Capability.ModifyEquipmentStatus,
        };

        var unlimited = new HashSet<Capability>(basic)
        {
            Capability.ManageUserEquipmentSelection,
            Capability.ManageCustomEquipment,
            Capability.UploadCustomEquipmentMedia
        };

        return new Dictionary<SubscriptionPlan, HashSet<Capability>>
        {
            [SubscriptionPlan.Trial] = trial,
            [SubscriptionPlan.Basic] = basic,
            [SubscriptionPlan.Unlimited] = unlimited
        };
    }
}