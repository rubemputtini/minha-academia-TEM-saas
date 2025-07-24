using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Auth;

public class CoachRegisterRequest
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    [StringLength(80, ErrorMessage = "O nome deve ter no máximo 80 caracteres.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número de telefone do usuário é obrigatório.")]
    [Phone(ErrorMessage = "O formato do número de telefone é inválido.")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "A rua é obrigatória.")]
    [StringLength(100, ErrorMessage = "A rua deve ter no máximo 100 caracteres.")]
    public string Street { get; set; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório.")]
    [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres.")]
    public string Number { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres.")]
    public string? Complement { get; set; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [StringLength(60, ErrorMessage = "O bairro deve ter no máximo 60 caracteres.")]
    public string Neighborhood { get; set; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(60, ErrorMessage = "A cidade deve ter no máximo 60 caracteres.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(50, ErrorMessage = "O estado deve ter no máximo 50 caracteres.")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "O país é obrigatório.")]
    [StringLength(50, ErrorMessage = "O país deve ter no máximo 50 caracteres.")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [StringLength(20, ErrorMessage = "O CEP deve ter no máximo 20 caracteres.")]
    public string PostalCode { get; set; } = string.Empty;

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}