namespace MinhaAcademiaTEM.Application.Services.Webhooks;

public interface IStripeWebhookService
{
    Task HandleAsync(string payload, string? signature);
}