using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class CoachRepository(ApplicationDbContext dbContext) : ICoachRepository
{
    public async Task<Coach?> GetByIdAsync(Guid id) =>
        await dbContext.Coaches.FindAsync(id);

    public async Task<int> GetTotalCoachesAsync() =>
        await dbContext.Coaches.CountAsync();

    public async Task AddAsync(Coach coach)
    {
        dbContext.Coaches.Add(coach);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Coach coach)
    {
        dbContext.Coaches.Update(coach);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Coach coach)
    {
        dbContext.Coaches.Remove(coach);
        await dbContext.SaveChangesAsync();
    }

    public async Task<bool> ExistsSlugAsync(string slug)
    {
        return await dbContext.Coaches.AnyAsync(c => c.Slug == slug);
    }

    public async Task<Coach?> GetBySlugAsync(string slug)
    {
        return await dbContext.Coaches.FirstOrDefaultAsync(c => c.Slug == slug);
    }

    public async Task<List<Coach>> SearchAsync(string? search, int skip, int take)
    {
        var query = dbContext.Coaches
            .Include(c => c.User)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Name.Contains(search) ||
                u.Email.Contains(search));

        return await query
            .AsNoTracking()
            .OrderBy(u => u.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync();
    }

    public async Task<int> CountAsync(string? search)
    {
        var query = dbContext.Coaches.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(u =>
                u.Name.Contains(search) ||
                u.Email.Contains(search));

        return await query.CountAsync();
    }
}