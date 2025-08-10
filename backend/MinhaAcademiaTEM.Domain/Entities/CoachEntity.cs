namespace MinhaAcademiaTEM.Domain.Entities;

public abstract class CoachEntity : BaseEntity
{
    public Guid CoachId { get; private set; }

    protected CoachEntity()
    {
    }

    protected CoachEntity(Guid coachId) => CoachId = coachId;
}