using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface ICoachRepository
{
    Task<Coach?> GetByIdAsync(Guid id);
    Task<Coach?> GetByEmailAsync(string email);
    Task<List<Coach>> GetAllAsync();
    Task AddAsync(Coach coach);
    Task UpdateAsync(Coach coach);
    Task DeleteAsync(Coach coach);
    Task<bool> ExistsSlugAsync(string slug);
    Task<Coach?> GetBySlugAsync(string slug);
}