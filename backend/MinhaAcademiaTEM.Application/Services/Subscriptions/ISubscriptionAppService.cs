using MinhaAcademiaTEM.Application.DTOs.Admin;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public interface ISubscriptionAppService
{
    Task<UpdateCoachSubscriptionResponse> ScheduleCancelAtPeriodEndAsync(Guid userId);
    Task<UpdateCoachSubscriptionResponse> UndoScheduledCancelAsync(Guid userId);
}