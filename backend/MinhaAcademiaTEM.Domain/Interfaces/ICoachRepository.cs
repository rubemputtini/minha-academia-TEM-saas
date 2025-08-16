using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface ICoachRepository : IBaseRepository<Coach>
{
    Task<Coach?> GetByUserIdAsync(Guid userId);
    Task<Coach?> GetByStripeCustomerIdAsync(string customerId);
    Task<int> GetTotalCoachesAsync();
    Task<bool> ExistsSlugAsync(string slug);
    Task<Coach?> GetBySlugAsync(string slug);
    Task<List<Coach>> SearchAsync(string? search, int skip, int take);
    Task<int> CountAsync(string? search);
}