using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Account;

public class UpdateMyUserRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O nome da academia é obrigatório.")]
    [StringLength(100, ErrorMessage = "O nome da academia deve ter no máximo 100 caracteres.")]
    public string GymName { get; set; } = string.Empty;

    [Required(ErrorMessage = "A localização da academia é obrigatória.")]
    [StringLength(100, ErrorMessage = "A localização da academia deve ter no máximo 100 caracteres.")]
    public string GymLocation { get; set; } = string.Empty;
}