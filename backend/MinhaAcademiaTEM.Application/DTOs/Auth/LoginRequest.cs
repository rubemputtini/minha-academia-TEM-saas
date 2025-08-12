using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public sealed class LoginRequest
{
    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; init; } = string.Empty;
}