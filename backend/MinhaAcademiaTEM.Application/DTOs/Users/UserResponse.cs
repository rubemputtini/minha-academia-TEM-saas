namespace MinhaAcademiaTEM.Application.DTOs.Users;

public class UserResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public Guid? CoachId { get; set; }
    public string? CoachName { get; set; }
}