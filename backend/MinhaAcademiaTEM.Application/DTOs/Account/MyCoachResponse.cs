using MinhaAcademiaTEM.Application.DTOs.Common;

namespace MinhaAcademiaTEM.Application.DTOs.Account;

public sealed class MyCoachResponse
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;

    public string SubscriptionStatus { get; init; } = string.Empty;
    public string SubscriptionPlan { get; init; } = string.Empty;
    public DateTime? SubscriptionEndAt { get; init; }

    public string CoachCode { get; init; } = string.Empty;

    public AddressResponse Address { get; init; } = null!;
}