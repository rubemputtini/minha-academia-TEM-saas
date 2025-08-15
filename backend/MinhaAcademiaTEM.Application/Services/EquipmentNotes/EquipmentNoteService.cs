using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.EquipmentNotes;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.EquipmentNotes;

public class EquipmentNoteService(
    EntityLookup lookup,
    IEquipmentNoteRepository equipmentNoteRepository,
    ICurrentUserService currentUserService,
    IAppCacheService cacheService)
    : IEquipmentNoteService
{
    public async Task<EquipmentNoteResponse> GetByUserIdAsync(Guid userId)
    {
        var cacheKey = CacheKeys.UserEquipmentNotes(userId);

        if (cacheService.TryGetValue(cacheKey, out EquipmentNoteResponse cachedNotes))
            return cachedNotes;

        var user = await lookup.GetUserAsync(userId);
        var coach = await lookup.GetCoachByUserIdAsync(user.CoachId!.Value);

        if (coach.Id != currentUserService.GetUserId())
            throw new NotFoundException("Cliente não encontrado ou não pertence a este treinador.");

        var note = await equipmentNoteRepository.GetByUserIdAsync(userId, coach.Id);

        var response = new EquipmentNoteResponse
        {
            Message = note?.Message ?? string.Empty
        };

        cacheService.Set(cacheKey, response);

        return response;
    }

    public async Task<EquipmentNoteResponse> UpsertAsync(UpsertEquipmentNoteRequest request)
    {
        var userId = currentUserService.GetUserId();

        var user = await lookup.GetUserAsync(userId);
        var coach = await lookup.GetCoachByUserIdAsync(user.CoachId!.Value);

        var existing = await equipmentNoteRepository.GetByUserIdAsync(userId, coach.Id);

        if (existing == null)
        {
            var note = new EquipmentNote(coach.Id, userId, request.Message);
            await equipmentNoteRepository.AddAsync(note);
        }
        else
        {
            existing.Update(request.Message);
            await equipmentNoteRepository.UpdateAsync(existing);
        }

        cacheService.Remove(CacheKeys.UserEquipmentNotes(userId));

        var response = new EquipmentNoteResponse
        {
            Message = request.Message
        };

        return response;
    }
}