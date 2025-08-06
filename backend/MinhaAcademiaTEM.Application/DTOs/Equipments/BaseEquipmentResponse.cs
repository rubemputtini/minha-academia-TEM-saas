using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public class BaseEquipmentResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; set; }
}