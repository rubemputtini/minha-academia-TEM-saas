using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;

public sealed class SaveEquipmentSelectionsRequest
{
    [Required(ErrorMessage = "Os IDs dos equipamentos disponíveis são obrigatórios.")]
    [MinLength(1, ErrorMessage = "Informe pelo menos 1 equipamento disponível.")]
    public IReadOnlyList<Guid> AvailableEquipmentIds { get; init; } = [];
}