using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public class BaseEquipmentResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string PhotoUrl { get; init; } = string.Empty;
    public string VideoUrl { get; init; } = string.Empty;

    public MuscleGroup MuscleGroup { get; init; }
}