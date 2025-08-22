using MinhaAcademiaTEM.Application.DTOs.Subscriptions;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public interface ISubscriptionSummaryReader
{
    Task<SubscriptionSummaryResponse> FromSubscriptionIdAsync(string subscriptionId);
}