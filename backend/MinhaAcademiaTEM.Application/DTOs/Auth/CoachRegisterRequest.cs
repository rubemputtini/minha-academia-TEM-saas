using System.ComponentModel.DataAnnotations;
using MinhaAcademiaTEM.Application.DTOs.Common;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public sealed class CoachRegisterRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres.")]
    public string Name { get; init; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "O número de telefone do usuário é obrigatório.")]
    [Phone(ErrorMessage = "O formato do número de telefone é inválido.")]
    public string PhoneNumber { get; init; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "O endereço é obrigatório.")]
    public AddressRequest Address { get; init; } = null!;
}