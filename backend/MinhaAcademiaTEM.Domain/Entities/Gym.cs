namespace MinhaAcademiaTEM.Domain.Entities;

public class Gym : CoachEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Location { get; private set; } = string.Empty;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    protected Gym()
    {
    }

    public Gym(Guid coachId, string name, string location, Guid userId) :
        base(coachId)
    {
        Name = name.Trim();
        Location = location.Trim();
        UserId = userId;
    }

    public void UpdateInfo(string newName, string newLocation)
    {
        Name = newName.Trim();
        Location = newLocation.Trim();
    }
}