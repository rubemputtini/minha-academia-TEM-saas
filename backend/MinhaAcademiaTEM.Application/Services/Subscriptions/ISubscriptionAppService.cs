using MinhaAcademiaTEM.Application.DTOs.Admin;
using MinhaAcademiaTEM.Application.DTOs.Subscriptions;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public interface ISubscriptionAppService
{
    Task<SubscriptionSummaryResponse> GetMySubscriptionSummaryAsync(Guid userId);
    Task<UpdateCoachSubscriptionResponse> ScheduleCancelAtPeriodEndAsync(Guid userId);
    Task<UpdateCoachSubscriptionResponse> UndoScheduledCancelAsync(Guid userId);
}