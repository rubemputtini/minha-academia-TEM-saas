namespace MinhaAcademiaTEM.Domain.Configuration;

public sealed class StripeApiConfiguration
{
    public string SecretKey { get; init; } = string.Empty;
    public string WebhookSecret { get; init; } = string.Empty;
    public string SuccessUrl { get; init; } = string.Empty;
    public string SignupSuccessUrl { get; init; } = string.Empty;
    public string CancelUrl { get; init; } = string.Empty;
    public Dictionary<string, string> PriceIds { get; init; } = [];
}