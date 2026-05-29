using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface ICoachRepository : IBaseRepository<Coach>
{
    Task<Coach?> GetByUserIdAsync(Guid userId);
    Task<Coach?> GetByStripeCustomerIdAsync(string customerId);
    Task<Coach?> GetByStripeSubscriptionIdAsync(string subscriptionId);
    Task<int> GetTotalCoachesAsync();
    Task<bool> ExistsSlugAsync(string slug);
    Task<Coach?> GetBySlugAsync(string slug);
    Task<List<Coach>> SearchAsync(string? search, int skip, int take);
    Task<int> CountAsync(string? search);
    Task<Dictionary<SubscriptionStatus, int>> GetCountsByStatusAsync();
    Task<int> CountNewThisMonthAsync();
    Task<int> CountWithoutClientsAsync(int daysThreshold);
    Task<Dictionary<SubscriptionPlan, int>> GetActiveCountsByPlanAsync();
}