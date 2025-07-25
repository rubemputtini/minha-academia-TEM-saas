using MinhaAcademiaTEM.Application.DTOs.Equipments;

namespace MinhaAcademiaTEM.Application.DTOs.Users;

public class UserDetailsResponse : UserResponse
{
    public string GymName { get; set; } = string.Empty;
    public string GymLocation { get; set; } = string.Empty;

    public List<EquipmentResponse> SelectedEquipments { get; set; } = [];
}