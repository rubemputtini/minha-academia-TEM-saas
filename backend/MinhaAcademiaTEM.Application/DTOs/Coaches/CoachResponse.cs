using MinhaAcademiaTEM.Application.DTOs.Common;

namespace MinhaAcademiaTEM.Application.DTOs.Coaches;

public class CoachResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public bool IsActive { get; set; }
    public string SubscriptionStatus { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = string.Empty;
    public DateTime? SubscriptionEndAt { get; set; }

    public int ClientsCount { get; set; }
}