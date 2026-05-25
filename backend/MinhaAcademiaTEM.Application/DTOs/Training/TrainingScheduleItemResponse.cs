namespace MinhaAcademiaTEM.Application.DTOs.Training;

public sealed class TrainingScheduleItemResponse
{
    public Guid UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string GymName { get; init; } = string.Empty;
    public DateTime? NextTrainingChangeAt { get; init; }
}
