using MinhaAcademiaTEM.Application.DTOs.Equipments;

namespace MinhaAcademiaTEM.Application.DTOs.Users;

public sealed class UserDetailsResponse : UserResponse
{
    public string GymName { get; init; } = string.Empty;
    public string GymCity { get; init; } = string.Empty;
    public string GymCountry { get; init; } = string.Empty;

    public List<EquipmentResponse> SelectedEquipments { get; init; } = [];
}