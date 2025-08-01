using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public class ResetPasswordRequest
{
    [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O token do usuário é obrigatório.")]
    public string Token { get; set; } = string.Empty;

    [Required(ErrorMessage = "A nova senha do usuário é obrigatória.")]
    public string NewPassword { get; set; } = string.Empty;
}