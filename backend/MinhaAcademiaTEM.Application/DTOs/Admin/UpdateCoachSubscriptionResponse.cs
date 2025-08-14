using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Admin;

public sealed class UpdateCoachSubscriptionResponse
{
    public Guid CoachId { get; init; }
    public SubscriptionStatus SubscriptionStatus { get; init; }
    public SubscriptionPlan SubscriptionPlan { get; init; }
    public DateTime? SubscriptionEndAt { get; init; }
}