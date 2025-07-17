namespace MinhaAcademiaTEM.Domain.Entities;

public class EquipmentSelection : CoachEntity
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;

    public bool IsAvailable { get; set; } = false;
}