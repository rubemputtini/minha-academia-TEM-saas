namespace MinhaAcademiaTEM.Application.DTOs.Billing;

public sealed class AddressPrefillResponse
{
    public string Street { get; init; } = string.Empty;
    public string? Number { get; init; }
    public string? Complement { get; init; }
    public string? Neighborhood { get; init; }
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;
}