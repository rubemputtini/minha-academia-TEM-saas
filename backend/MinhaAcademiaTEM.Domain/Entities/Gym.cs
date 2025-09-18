namespace MinhaAcademiaTEM.Domain.Entities;

public class Gym : CoachEntity
{
    public string Name { get; private set; } = string.Empty;
    public string City { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    protected Gym()
    {
    }

    public Gym(Guid coachId, string name, string city, string country, Guid userId) :
        base(coachId)
    {
        Name = name.Trim();
        City = city.Trim();
        Country = country.Trim();
        UserId = userId;
    }

    public void UpdateInfo(string newName, string newCity, string newCountry)
    {
        Name = newName.Trim();
        City = newCity.Trim();
        Country = newCountry.Trim();
    }
}