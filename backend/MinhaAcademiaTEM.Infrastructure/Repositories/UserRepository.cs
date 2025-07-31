using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id) =>
        await dbContext.Users.FindAsync(id);

    public async Task<int> GetTotalUsersAsync() =>
        await dbContext.Users.CountAsync();

    public async Task<List<User>> GetAllByCoachIdAsync(Guid coachId) =>
        await dbContext.Users
            .AsNoTracking()
            .Where(u => u.CoachId == coachId)
            .ToListAsync();

    public async Task AddAsync(User user)
    {
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(User user)
    {
        dbContext.Users.Update(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User user)
    {
        dbContext.Users.Remove(user);
        await dbContext.SaveChangesAsync();
    }

    public async Task<List<User>> SearchAsync(string? search, int skip, int take)
    {
        var query = dbContext.Users
            .Include(u => u.Coach)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Name.Contains(search) ||
                u.Email!.Contains(search));

        return await query
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<List<User>> SearchByCoachAsync(Guid coachId, string? search, int skip, int take)
    {
        var query = dbContext.Users
            .Include(u => u.Coach)
            .Where(u => u.CoachId == coachId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Name.Contains(search) ||
                u.Email!.Contains(search));

        return await query
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> CountAsync(string? search)
    {
        var query = dbContext.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Name.Contains(search) ||
                u.Email!.Contains(search));

        return await query.CountAsync();
    }

    public async Task<int> CountByCoachAsync(Guid coachId, string? search)
    {
        var query = dbContext.Users
            .Where(u => u.CoachId == coachId);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Name.Contains(search) ||
                u.Email!.Contains(search));

        return await query.CountAsync();
    }

    public async Task<Dictionary<Guid, int>> GetClientsCountForCoachesAsync(List<Guid> coachIds) =>
        await dbContext.Users
            .Where(u => coachIds.Contains(u.CoachId!.Value))
            .GroupBy(u => u.CoachId)
            .Select(g => new { CoachId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.CoachId!.Value, g => g.Count);
}