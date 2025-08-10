using MinhaAcademiaTEM.Application.DTOs.Common;

namespace MinhaAcademiaTEM.Application.DTOs.Coaches;

public class CoachResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;

    public bool IsActive { get; init; }
    public string SubscriptionStatus { get; init; } = string.Empty;
    public string SubscriptionPlan { get; init; } = string.Empty;
    public DateTime? SubscriptionEndAt { get; init; }

    public int ClientsCount { get; init; }
}