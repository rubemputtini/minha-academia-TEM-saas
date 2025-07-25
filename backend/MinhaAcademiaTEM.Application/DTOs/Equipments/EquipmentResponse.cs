namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public class EquipmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;
    public string MuscleGroup { get; set; } = string.Empty;
    public Guid BaseEquipmentId { get; set; }
}