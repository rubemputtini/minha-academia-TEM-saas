using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class CoachRepository(ApplicationDbContext dbContext) : ICoachRepository
{
    public async Task<Coach?> GetByIdAsync(Guid id) =>
        await dbContext.Coaches.FindAsync(id);

    public async Task<Coach?> GetByEmailAsync(string email) =>
        await dbContext.Coaches.FirstOrDefaultAsync(c => c.Email == email);

    public async Task<List<Coach>> GetAllAsync() =>
        await dbContext.Coaches.ToListAsync();

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
}