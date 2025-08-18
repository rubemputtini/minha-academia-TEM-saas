using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public sealed class CoachRegisterAfterPaymentRequest
{
    [Required(ErrorMessage = "O ID da sessão é obrigatório.")]
    public string SessionId { get; init; } = string.Empty;
    
    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; init; } = string.Empty;
}