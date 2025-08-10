using MinhaAcademiaTEM.Application.DTOs.Equipments;

namespace MinhaAcademiaTEM.Application.DTOs.Users;

public class UserDetailsResponse : UserResponse
{
    public string GymName { get; init; } = string.Empty;
    public string GymLocation { get; init; } = string.Empty;

    public List<EquipmentResponse> SelectedEquipments { get; init; } = [];
}