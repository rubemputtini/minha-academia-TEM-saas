namespace MinhaAcademiaTEM.Domain.Entities;

public class Coach : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } =  string.Empty;
    public string Slug { get; set; } = string.Empty;

    public ICollection<Gym> Gyms { get; set; } = new List<Gym>();
    public ICollection<Equipment> Equipments { get; set; } = new List<Equipment>();
}