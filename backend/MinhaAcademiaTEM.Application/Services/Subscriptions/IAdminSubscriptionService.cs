using MinhaAcademiaTEM.Application.DTOs.Admin;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public interface IAdminSubscriptionService
{
    Task<UpdateCoachSubscriptionResponse> CancelNowAsync(Guid coachId);
}