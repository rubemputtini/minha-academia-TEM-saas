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
}