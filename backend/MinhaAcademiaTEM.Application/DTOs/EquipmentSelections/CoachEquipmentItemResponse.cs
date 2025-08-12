using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;

public class CoachEquipmentItemResponse
{
    public Guid EquipmentId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string VideoUrl { get; init; } = string.Empty;
    public MuscleGroup MuscleGroup { get; init; }
}