namespace MinhaAcademiaTEM.Application.DTOs.Common;

public class AddressResponse
{
    public string Street { get; init; } = string.Empty;
    public string Number { get; init; } = string.Empty;
    public string? Complement { get; init; }
    public string Neighborhood { get; init; } = string.Empty;
    public string City { get; init; } = string.Empty;
    public string State { get; init; } = string.Empty;
    public string Country { get; init; } = string.Empty;
    public string PostalCode { get; init; } = string.Empty;

    public double? Latitude { get; init; }
    public double? Longitude { get; init; }
}