using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public class ToggleEquipmentRequest
{
    [Required]
    public bool IsActive { get; set; }
}