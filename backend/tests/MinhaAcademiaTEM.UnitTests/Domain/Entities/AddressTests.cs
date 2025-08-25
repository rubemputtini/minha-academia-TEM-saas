using FluentAssertions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Domain.Entities;

public class AddressTests
{
    private static Address NewAddress(
        string street = "Rua X",
        string number = "123",
        string? complement = null,
        string neighborhood = "Centro",
        string city = "Porto",
        string state = "pt",
        string country = "pt",
        string postalCode = "4000-000",
        double? latitude = null,
        double? longitude = null,
        Guid? coachId = null
    ) => new(
        street, number, complement, neighborhood, city, state, country,
        postalCode, latitude, longitude, coachId ?? Guid.NewGuid()
    );

    [Fact]
    public void Constructor_Should_Trim_Fields_Uppercase_State_And_Country_And_Set_CoachId()
    {
        var coachId = Guid.NewGuid();

        var a = NewAddress(
            street: " Rua X  ",
            number: "  123 ",
            complement: "  Apt 12  ",
            neighborhood: "  Centro  ",
            city: "  Porto  ",
            state: "pt",
            country: "br",
            postalCode: "  4000-000 ",
            latitude: 10.5,
            longitude: -8.6,
            coachId: coachId
        );

        a.Street.Should().Be("Rua X");
        a.Number.Should().Be("123");
        a.Complement.Should().Be("Apt 12");
        a.Neighborhood.Should().Be("Centro");
        a.City.Should().Be("Porto");
        a.State.Should().Be("PT");
        a.Country.Should().Be("BR");
        a.PostalCode.Should().Be("4000-000");
        a.Latitude.Should().Be(10.5);
        a.Longitude.Should().Be(-8.6);
        a.CoachId.Should().Be(coachId);
    }

    [Fact]
    public void UpdateAddress_Should_Trim_Uppercase_And_Update_All_Fields()
    {
        var a = NewAddress(complement: null, state: "pt", country: "pt", postalCode: "1000-000");

        a.UpdateAddress(
            street: "  Nova Rua  ",
            number: "  99 ",
            complement: "  Bloco B ",
            neighborhood: "  Bairro  ",
            city: "  Lisboa ",
            state: "br",
            country: "us",
            postalCode: "  2000-000 ",
            latitude: 1.1,
            longitude: 2.2
        );

        a.Street.Should().Be("Nova Rua");
        a.Number.Should().Be("99");
        a.Complement.Should().Be("Bloco B");
        a.Neighborhood.Should().Be("Bairro");
        a.City.Should().Be("Lisboa");
        a.State.Should().Be("BR");
        a.Country.Should().Be("US");
        a.PostalCode.Should().Be("2000-000");
        a.Latitude.Should().Be(1.1);
        a.Longitude.Should().Be(2.2);
    }

    [Fact]
    public void UpdateAddress_Should_Set_Complement_To_Null_When_Whitespace()
    {
        var a = NewAddress(complement: "Apt 1");

        a.UpdateAddress(
            street: a.Street,
            number: a.Number,
            complement: "   ",
            neighborhood: a.Neighborhood,
            city: a.City,
            state: a.State,
            country: a.Country,
            postalCode: a.PostalCode,
            latitude: a.Latitude,
            longitude: a.Longitude
        );

        a.Complement.Should().BeNull();
    }

    [Fact]
    public void UpdateAddress_Should_Not_Change_CoachId()
    {
        var coachId = Guid.NewGuid();
        var a = NewAddress(coachId: coachId);

        a.UpdateAddress("Rua Y", "1", null, "Centro", "Porto", "pt", "pt", "4000-000", 0.0, 0.0);

        a.CoachId.Should().Be(coachId);
    }
}