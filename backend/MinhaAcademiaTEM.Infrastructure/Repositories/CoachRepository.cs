using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class CoachRepository(ApplicationDbContext dbContext) : BaseRepository<Coach>(dbContext), ICoachRepository
{
    public async Task<Coach?> GetByUserIdAsync(Guid userId) =>
        await dbContext.Coaches
            .Include(c => c.User)
            .Include(c => c.Address)
            .FirstOrDefaultAsync(c => c.User!.Id == userId);

    public async Task<Coach?> GetByStripeCustomerIdAsync(string customerId) =>
        await dbContext.Coaches
            .FirstOrDefaultAsync(c => c.StripeCustomerId == customerId);

    public async Task<Coach?> GetByStripeSubscriptionIdAsync(string subscriptionId) =>
        await dbContext.Coaches
            .FirstOrDefaultAsync(c => c.StripeSubscriptionId == subscriptionId);

    public async Task<int> GetTotalCoachesAsync() =>
        await dbContext.Coaches.CountAsync();

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