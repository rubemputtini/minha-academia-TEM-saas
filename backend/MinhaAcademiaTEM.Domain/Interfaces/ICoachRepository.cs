using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface ICoachRepository
{
    Task<Coach?> GetByIdAsync(Guid id);
    Task<Coach?> GetByEmailAsync(string email);
    Task<List<Coach>> GetAllAsync();
    Task<int> GetTotalCoachesAsync();
    Task AddAsync(Coach coach);
    Task UpdateAsync(Coach coach);
    Task DeleteAsync(Coach coach);
    Task<bool> ExistsSlugAsync(string slug);
    Task<Coach?> GetBySlugAsync(string slug);
    Task<List<Coach>> SearchAsync(string? search, int skip, int take);
    Task<int> CountAsync(string? search);
}