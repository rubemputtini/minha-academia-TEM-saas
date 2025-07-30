using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
    Task<User?> GetByEmailAsync(string email);
    Task<List<User>> GetAllAsync();
    Task<int> GetTotalUsersAsync();
    Task<List<User>> GetAllByCoachIdAsync(Guid coachId);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(User user);
    Task<List<User>> SearchAsync(string? search, int skip, int take);
    Task<List<User>> SearchByCoachAsync(Guid coachId, string? search, int skip, int take);
    Task<int> CountAsync(string? search);
    Task<int> CountByCoachAsync(Guid coachId, string? search = null);
    Task<Dictionary<Guid, int>> GetClientsCountForCoachesAsync(List<Guid> coachIds);
}