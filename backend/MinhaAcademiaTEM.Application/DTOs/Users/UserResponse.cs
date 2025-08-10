namespace MinhaAcademiaTEM.Application.DTOs.Users;

public class UserResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;

    public Guid? CoachId { get; init; }
    public string? CoachName { get; init; }
}