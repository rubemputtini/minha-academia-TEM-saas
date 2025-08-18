namespace MinhaAcademiaTEM.Application.DTOs.Subscriptions;

public sealed class UpdateSubscriptionRequest
{
    public required string CustomerId { get; init; } = string.Empty;
    public required string SubscriptionId { get; init; } = string.Empty;
    public required string PriceId { get; init; } = string.Empty;
    public required string StripeStatus { get; init; } = string.Empty;
}