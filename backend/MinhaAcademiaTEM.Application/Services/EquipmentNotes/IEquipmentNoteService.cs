using MinhaAcademiaTEM.Application.DTOs.EquipmentNotes;

namespace MinhaAcademiaTEM.Application.Services.EquipmentNotes;

public interface IEquipmentNoteService
{
    Task<EquipmentNoteResponse> GetByUserIdAsync(Guid userId);
    Task<EquipmentNoteResponse> UpsertAsync(UpsertEquipmentNoteRequest request);
}