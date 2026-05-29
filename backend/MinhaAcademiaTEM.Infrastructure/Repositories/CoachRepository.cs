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

        return await (
            from c in query
            join r in dbContext.ReferralAccounts on c.Id equals r.CoachId into referrals
            from r in referrals.DefaultIfEmpty()
            orderby
                c.SubscriptionStatus == SubscriptionStatus.Active ? 0 :
                c.SubscriptionStatus == SubscriptionStatus.Trial ? 1 :
                c.SubscriptionStatus == SubscriptionStatus.PastDue ? 2 : 3,
                c.SubscriptionStatus == SubscriptionStatus.Active
                    ? (c.SubscriptionPlan == SubscriptionPlan.Unlimited ? 0 :
                       c.SubscriptionPlan == SubscriptionPlan.Basic ? 1 : 2) : 0,
                r == null ? 0 : r.TotalCreditsEarned descending,
                c.Name
            select c)
            .AsNoTracking()
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

    public async Task<Dictionary<SubscriptionStatus, int>> GetCountsByStatusAsync() =>
        await dbContext.Coaches
            .AsNoTracking()
            .GroupBy(c => c.SubscriptionStatus)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Status, x => x.Count);

    public async Task<int> CountNewThisMonthAsync()
    {
        var firstDayOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        
        return await dbContext.Coaches.CountAsync(c => c.CreatedAt >= firstDayOfMonth);
    }

    public async Task<int> CountWithoutClientsAsync(int daysThreshold)
    {
        var cutoff = DateTime.UtcNow.AddDays(-daysThreshold);
        
        return await dbContext.Coaches.CountAsync(c =>
            c.CreatedAt <= cutoff &&
            !dbContext.Users.Any(u => u.CoachId == c.Id));
    }

    public async Task<Dictionary<SubscriptionPlan, int>> GetActiveCountsByPlanAsync() =>
        await dbContext.Coaches
            .AsNoTracking()
            .Where(c => c.SubscriptionStatus == SubscriptionStatus.Active)
            .GroupBy(c => c.SubscriptionPlan)
            .Select(g => new { Plan = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Plan, x => x.Count);
}