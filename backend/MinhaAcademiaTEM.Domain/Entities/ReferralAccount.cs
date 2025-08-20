namespace MinhaAcademiaTEM.Domain.Entities;

public class ReferralAccount : BaseEntity
{
    public Guid CoachId { get; private set; }
    public Coach Coach { get; private set; } = null!;
    public int CreditsAvailable { get; private set; }
    public int? LastAppliedPeriod { get; private set; }
    public DateTime? UpdatedAt { get; private set; } = DateTime.UtcNow;

    protected ReferralAccount()
    {
    }

    public ReferralAccount(Guid coachId) => CoachId = coachId;

    public void AddCredit(int credits = 1)
    {
        CreditsAvailable += credits;
        UpdatedAt = DateTime.UtcNow;
    }

    public bool CanApplyForPeriod(DateTime periodStartUtc)
    {
        var key = periodStartUtc.Year * 100 + periodStartUtc.Month;

        return CreditsAvailable > 0 && LastAppliedPeriod != key;
    }

    public void MarkApplied(DateTime periodStartUtc)
    {
        CreditsAvailable = Math.Max(0, CreditsAvailable - 1);
        LastAppliedPeriod = periodStartUtc.Year * 100 + periodStartUtc.Month;
        UpdatedAt = DateTime.UtcNow;
    }
}