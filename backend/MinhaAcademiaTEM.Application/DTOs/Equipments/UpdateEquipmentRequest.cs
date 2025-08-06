using System.ComponentModel.DataAnnotations;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.DTOs.Equipments;

public class UpdateEquipmentRequest
{
    [Required(ErrorMessage = "O nome do equipamento é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome do equipamento deve ter no máximo 100 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O link da imagem do equipamento é obrigatório.")]
    [StringLength(500, ErrorMessage = "O link da imagem do equipamento deve ter no máximo 500 caracteres.")]
    [Url(ErrorMessage = "O link da imagem deve ser uma URL válida.")]
    public string PhotoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "O link do vídeo do equipamento é obrigatório.")]
    [StringLength(500, ErrorMessage = "O link do vídeo do equipamento deve ter no máximo 500 caracteres.")]
    [Url(ErrorMessage = "O link do vídeo deve ser uma URL válida.")]
    public string VideoUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "A categoria do equipamento é obrigatória.")]
    public MuscleGroup MuscleGroup { get; set; }

    [Required(ErrorMessage = "O ID do equipamento base é obrigatório.")]
    public Guid BaseEquipmentId { get; set; }
}