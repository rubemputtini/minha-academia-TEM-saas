namespace MinhaAcademiaTEM.Application.DTOs.Subscriptions;

public sealed class SubscriptionSummaryResponse
{
    public long AmountInCents { get; init; }
    public string Currency { get; init; } = string.Empty;
    public DateTime NextBillingUtc { get; init; } = DateTime.UtcNow;
    public string PriceId { get; init; } = string.Empty;
}