using Microsoft.EntityFrameworkCore;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Infrastructure.Persistence;

namespace MinhaAcademiaTEM.Infrastructure.Repositories;

public class ReferralAccountRepository(ApplicationDbContext dbContext)
    : BaseRepository<ReferralAccount>(dbContext), IReferralAccountRepository
{
    public async Task<ReferralAccount?> GetByCoachIdAsync(Guid coachId) =>
        await dbContext.ReferralAccounts
            .FirstOrDefaultAsync(a => a.CoachId == coachId);

    public async Task<Dictionary<Guid, int>> GetReferralCountsForCoachesAsync(List<Guid> coachIds) =>
        await dbContext.ReferralAccounts
            .AsNoTracking()
            .Where(r => coachIds.Contains(r.CoachId))
            .ToDictionaryAsync(r => r.CoachId, r => r.TotalCreditsEarned);
}