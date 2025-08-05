namespace MinhaAcademiaTEM.Domain.Entities;

public class Equipment : CoachEntity
{
    public string Name { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; set; }

    public Guid BaseEquipmentId { get; set; }
    public BaseEquipment BaseEquipment { get; set; } = null!;
    public bool IsActive { get; set; } = true;
}