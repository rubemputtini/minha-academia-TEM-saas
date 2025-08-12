using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;

public sealed class UserEquipmentItemResponse
{
    public Guid EquipmentId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string VideoUrl { get; init; } = string.Empty;
    public MuscleGroup MuscleGroup { get; init; }
    public bool IsAvailable { get; init; }
}