namespace MinhaAcademiaTEM.Domain.Entities;

public class Equipment : CoachEntity
{
    public string Name { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty;
    public string VideoUrl { get; set; } = string.Empty;

    public MuscleGroup MuscleGroup { get; set; }
}