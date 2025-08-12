using System.ComponentModel.DataAnnotations;

namespace MinhaAcademiaTEM.Application.DTOs.Common;

public abstract class AddressRequest
{
    [Required(ErrorMessage = "A rua é obrigatória.")]
    [StringLength(100, ErrorMessage = "A rua deve ter no máximo 100 caracteres.")]
    public string Street { get; init; } = string.Empty;

    [Required(ErrorMessage = "O número é obrigatório.")]
    [StringLength(20, ErrorMessage = "O número deve ter no máximo 20 caracteres.")]
    public string Number { get; init; } = string.Empty;

    [StringLength(100, ErrorMessage = "O complemento deve ter no máximo 100 caracteres.")]
    public string? Complement { get; init; }

    [Required(ErrorMessage = "O bairro é obrigatório.")]
    [StringLength(60, ErrorMessage = "O bairro deve ter no máximo 60 caracteres.")]
    public string Neighborhood { get; init; } = string.Empty;

    [Required(ErrorMessage = "A cidade é obrigatória.")]
    [StringLength(60, ErrorMessage = "A cidade deve ter no máximo 60 caracteres.")]
    public string City { get; init; } = string.Empty;

    [Required(ErrorMessage = "O estado é obrigatório.")]
    [StringLength(50, ErrorMessage = "O estado deve ter no máximo 50 caracteres.")]
    public string State { get; init; } = string.Empty;

    [Required(ErrorMessage = "O país é obrigatório.")]
    [StringLength(50, ErrorMessage = "O país deve ter no máximo 50 caracteres.")]
    public string Country { get; init; } = string.Empty;

    [Required(ErrorMessage = "O CEP é obrigatório.")]
    [StringLength(20, ErrorMessage = "O CEP deve ter no máximo 20 caracteres.")]
    public string PostalCode { get; init; } = string.Empty;

    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}