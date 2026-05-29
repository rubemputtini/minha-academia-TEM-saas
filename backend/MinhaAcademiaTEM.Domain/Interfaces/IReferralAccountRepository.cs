using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface IReferralAccountRepository : IBaseRepository<ReferralAccount>
{
    Task<ReferralAccount?> GetByCoachIdAsync(Guid coachId);
    Task<Dictionary<Guid, int>> GetReferralCountsForCoachesAsync(List<Guid> coachIds);
}