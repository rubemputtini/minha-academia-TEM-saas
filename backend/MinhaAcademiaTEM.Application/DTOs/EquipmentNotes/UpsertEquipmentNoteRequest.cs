using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.EquipmentNotes;

public sealed class UpsertEquipmentNoteRequest
{
    [Required(ErrorMessage = "O campo de notas sobre os equipamentos é obrigatório.")]
    [StringLength(300, ErrorMessage = "As notas sobre os equipamentos devem ter no máximo 300 caracteres.")]
    public string Message { get; init; } = string.Empty;
}