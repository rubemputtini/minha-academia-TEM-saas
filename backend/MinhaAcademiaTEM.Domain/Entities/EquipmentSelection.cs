namespace MinhaAcademiaTEM.Domain.Entities;

public class EquipmentSelection : CoachEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public Guid EquipmentId { get; private set; }
    public Equipment Equipment { get; private set; } = null!;

    public bool IsAvailable { get; private set; } = false;

    protected EquipmentSelection()
    {
    }

    public EquipmentSelection(Guid coachId, Guid userId, Guid equipmentId, bool isAvailable = false)
        : base(coachId)
    {
        UserId = userId;
        EquipmentId = equipmentId;
        IsAvailable = isAvailable;
    }

    public void MarkAvailable() => IsAvailable = true;
    public void MarkUnavailable() => IsAvailable = false;
}