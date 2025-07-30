namespace MinhaAcademiaTEM.Domain.Entities;

public class Gym : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
}