using MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;

namespace MinhaAcademiaTEM.Application.Services.EquipmentSelections;

public interface IEquipmentSelectionService
{
    Task<List<UserEquipmentItemResponse>> GetUserViewAsync(Guid userId);
    Task<List<CoachEquipmentItemResponse>> GetCoachViewAsync(Guid userId);
    Task<List<UserEquipmentItemResponse>> SaveOwnAsync(Guid userId, SaveEquipmentSelectionsRequest request);
    Task<List<UserEquipmentItemResponse>> SaveClientAsync(Guid userId, SaveEquipmentSelectionsRequest request);
}