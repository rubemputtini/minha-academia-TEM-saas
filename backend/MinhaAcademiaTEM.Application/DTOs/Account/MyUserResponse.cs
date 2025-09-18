namespace MinhaAcademiaTEM.Application.DTOs.Account;

public sealed class MyUserResponse
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string GymName { get; init; } = string.Empty;
    public string GymCity { get; init; } = string.Empty;
    public string GymCountry { get; init; } = string.Empty;
}