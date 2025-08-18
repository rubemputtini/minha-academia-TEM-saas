namespace MinhaAcademiaTEM.Application.DTOs.Billing;

public sealed class CoachPreFillResponse
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string PhoneNumber { get; init; } = string.Empty;
    public AddressPrefillResponse Address { get; init; } = null!;

    public string SubscriptionPlan { get; init; } = string.Empty;
    public string StripeCustomerId { get; init; } = string.Empty;
    public string StripeSubscriptionId { get; init; } = string.Empty;
    public string ClientReferenceId { get; init; } = string.Empty;
}