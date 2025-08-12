using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public sealed class ForgotPasswordRequest
{
    [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
    public string Email { get; init; } = string.Empty;
}