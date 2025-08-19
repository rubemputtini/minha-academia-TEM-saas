using MinhaAcademiaTEM.Application.DTOs.Subscriptions;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public interface ISubscriptionSyncService
{
    Task UpdateAsync(UpdateSubscriptionRequest request);
}