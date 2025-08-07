using MinhaAcademiaTEM.Application.DTOs.Equipments;

namespace MinhaAcademiaTEM.Application.Services.Equipments;

public interface IBaseEquipmentService
{
    Task<List<BaseEquipmentResponse>> GetAllAsync();
    Task<BaseEquipmentResponse> GetByIdAsync(Guid id);
    Task<BaseEquipmentResponse> CreateAsync(CreateBaseEquipmentRequest equipment);
    Task<BaseEquipmentResponse> UpdateAsync(Guid id, UpdateBaseEquipmentRequest equipment);
    Task DeleteAsync(Guid id);
}