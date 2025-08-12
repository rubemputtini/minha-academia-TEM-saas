using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public sealed class ToggleEquipmentRequest
{
    [Required(ErrorMessage = "O novo status do equipamento é obrigatório.")]
    public bool IsActive { get; init; }
}