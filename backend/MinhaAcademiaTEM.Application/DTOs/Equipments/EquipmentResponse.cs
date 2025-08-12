using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public sealed class EquipmentResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string VideoUrl { get; init; } = string.Empty;
    public MuscleGroup MuscleGroup { get; init; }
    public Guid BaseEquipmentId { get; init; }
    public bool IsActive { get; init; }
}