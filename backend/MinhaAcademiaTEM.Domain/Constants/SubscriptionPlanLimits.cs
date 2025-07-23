using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Constants;

public static class SubscriptionPlanLimits
{
    private const int TrialMaxUsers = 1;
    private const int BasicMaxUsers = 5;
    private const int UnlimitedMaxUsers = int.MaxValue;

    public static int GetMaxUsers(SubscriptionPlan plan) => plan switch
    {
        SubscriptionPlan.Trial => TrialMaxUsers,
        SubscriptionPlan.Basic => BasicMaxUsers,
        SubscriptionPlan.Unlimited => UnlimitedMaxUsers,
        _ => 0
    };
}