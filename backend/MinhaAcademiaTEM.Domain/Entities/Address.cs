namespace MinhaAcademiaTEM.Domain.Entities;

public class Address : BaseEntity
{
    public string Street { get; private set; } = string.Empty;
    public string Number { get; private set; } = string.Empty;
    public string? Complement { get; private set; }
    public string Neighborhood { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string State { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public string PostalCode { get; private set; } = string.Empty;
    public double? Latitude { get; private set; }
    public double? Longitude { get; private set; }

    public Guid CoachId { get; private set; }
    public Coach Coach { get; private set; } = null!;

    protected Address()
    {
    }

    public Address(
        string street,
        string number,
        string? complement,
        string neighborhood,
        string city,
        string state,
        string country,
        string postalCode,
        double? latitude,
        double? longitude,
        Guid coachId)
    {
        Street = street.Trim();
        Number = number.Trim();
        Complement = string.IsNullOrWhiteSpace(complement) ? null : complement.Trim();
        Neighborhood = neighborhood.Trim();
        City = city.Trim();
        State = state.Trim().ToUpperInvariant();
        Country = country.Trim().ToUpperInvariant();
        PostalCode = postalCode.Trim();
        Latitude = latitude;
        Longitude = longitude;
        CoachId = coachId;
    }

    public void UpdateAddress(
        string street,
        string number,
        string? complement,
        string neighborhood,
        string city,
        string state,
        string country,
        string postalCode,
        double? latitude,
        double? longitude)
    {
        Street = street.Trim();
        Number = number.Trim();
        Complement = string.IsNullOrWhiteSpace(complement) ? null : complement.Trim();
        Neighborhood = neighborhood.Trim();
        City = city.Trim();
        State = state.Trim().ToUpperInvariant();
        Country = country.Trim().ToUpperInvariant();
        PostalCode = postalCode.Trim();
        Latitude = latitude;
        Longitude = longitude;
    }
}