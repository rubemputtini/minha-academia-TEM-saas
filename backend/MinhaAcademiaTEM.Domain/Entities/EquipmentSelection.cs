namespace MinhaAcademiaTEM.Domain.Entities;

public class EquipmentSelection : CoachEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid EquipmentId { get; private set; }
    public Equipment Equipment { get; private set; } = null!;

    protected EquipmentSelection()
    {
    }

    public EquipmentSelection(Guid coachId, Guid userId, Guid equipmentId)
        : base(coachId)
    {
        UserId = userId;
        EquipmentId = equipmentId;
    }
}