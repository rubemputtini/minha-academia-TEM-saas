using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Services.Equipments;

public interface IEquipmentService
{
    Task<List<EquipmentResponse>> GetAllByCoachIdAsync(Guid coachId);
    Task<List<EquipmentResponse>> GetActiveByCoachIdAsync(Guid coachId);
    Task<EquipmentResponse> GetByIdAsync(Guid id);
    Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request);
    Task<EquipmentResponse> UpdateAsync(Guid id, UpdateEquipmentRequest request);
    Task DeleteAsync(Guid id);
    Task<bool> ToggleActiveAsync(Guid id, ToggleEquipmentRequest request);
}