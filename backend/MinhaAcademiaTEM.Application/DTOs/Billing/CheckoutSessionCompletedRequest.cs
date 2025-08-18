namespace MinhaAcademiaTEM.Application.DTOs.Billing;

public sealed class CheckoutSessionCompletedRequest
{
    public string Mode { get; init; } = string.Empty;
    public string? ClientReferenceId { get; init; }
    public string? CustomerId { get; init; }
    public string? SubscriptionId { get; init; }
    public IReadOnlyDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
}