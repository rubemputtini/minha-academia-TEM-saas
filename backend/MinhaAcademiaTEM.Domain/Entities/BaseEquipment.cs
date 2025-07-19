namespace MinhaAcademiaTEM.Domain.Entities;

public class BaseEquipment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; set; }

    public ICollection<Equipment> Customizations { get; set; } = new List<Equipment>();
}