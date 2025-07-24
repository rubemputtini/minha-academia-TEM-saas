using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public class ForgotPasswordRequest
{
    [Required(ErrorMessage = "O e-mail do usuário é obrigatório.")]
    public string Email { get; set; } = string.Empty;
}