using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.UnitTests.Application.Helpers;

public static class TestData
{
    public static Address Address(
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
        Guid? coachId = null)
        => new(street, number, complement, neighborhood, city, state, country,
            postalCode, latitude, longitude, coachId ?? Guid.NewGuid());

    public static Coach Coach(
        Guid? id = null,
        string name = "Rubem",
        string email = "rubem@test.com",
        string slug = "rubem")
    {
        var coachId = id ?? Guid.NewGuid();
        var address = Address(coachId: coachId);
        return new Coach(coachId, name, email, slug, address);
    }

    public static User User(
        string name = "User",
        string email = "user@test.com",
        Guid? id = null,
        Guid? coachId = null)
    {
        var u = new User(name, email);
        if (id.HasValue) u.Id = id.Value;
        if (coachId.HasValue) u.AssignCoach(coachId.Value);
        return u;
    }

    public static BaseEquipment BaseEquipment(string name = "Leg Press",
        string photoUrl = "https://example.com/photo.jpg", string videoUrl = "https://example.com/video.mp4",
        MuscleGroup group = MuscleGroup.Pernas) =>
        new(name, photoUrl, videoUrl, group);

    public static Equipment Equipment(
        string name = "Leg Press",
        string videoUrl = "https://example.com/video.mp4",
        MuscleGroup muscleGroup = MuscleGroup.Pernas,
        Guid? baseEquipmentId = null,
        Guid? coachId = null)
        => new(name, videoUrl, muscleGroup,
            baseEquipmentId ?? Guid.NewGuid(),
            coachId ?? Guid.NewGuid());

    public static Gym Gym(Guid? coachId = null, string name = "Academia", string location = "Portugal",
        Guid? userId = null) =>
        new(coachId ?? Guid.NewGuid(), name, location, userId ?? Guid.NewGuid());

    public static EquipmentSelection EquipmentSelection(
        Guid? coachId = null,
        Guid? userId = null,
        Guid? equipmentId = null)
        => new(coachId ?? Guid.NewGuid(),
            userId ?? Guid.NewGuid(),
            equipmentId ?? Guid.NewGuid());

    public static EquipmentNote EquipmentNote(
        Guid? coachId = null,
        Guid? userId = null,
        string message = "  Bom trabalho  ")
        => new(coachId ?? Guid.NewGuid(),
            userId ?? Guid.NewGuid(),
            message);

    public static ReferralAccount ReferralAccount(Guid coachId = default) => new(coachId);
}