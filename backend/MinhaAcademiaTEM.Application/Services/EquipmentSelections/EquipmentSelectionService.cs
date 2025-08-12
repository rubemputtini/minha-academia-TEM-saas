using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.EquipmentSelections;

public class EquipmentSelectionService(
    EntityLookup lookup,
    AccessChecks access,
    IEquipmentRepository equipmentRepository,
    IEquipmentSelectionRepository equipmentSelectionRepository,
    IAppCacheService cacheService)
    : IEquipmentSelectionService
{
    public async Task<List<UserEquipmentItemResponse>> GetUserViewAsync(Guid userId)
    {
        var cacheKey = CacheKeys.UserEquipmentSelections(userId);

        if (cacheService.TryGetValue(cacheKey, out List<UserEquipmentItemResponse>? cachedEquipmentSelections))
            return cachedEquipmentSelections;

        var user = await lookup.GetUserAsync(userId);
        var coach = await lookup.GetCoachAsync(user.CoachId!.Value);

        var equipments = await equipmentRepository.GetActiveByCoachIdAsync(coach.Id);
        var selections = await equipmentSelectionRepository.GetByUserAsync(coach.Id, userId);
        var selectedSet = selections.Select(s => s.EquipmentId).ToHashSet();

        var response = equipments
            .Select(e => new UserEquipmentItemResponse
            {
                EquipmentId = e.Id,
                Name = e.Name,
                VideoUrl = e.VideoUrl,
                MuscleGroup = e.MuscleGroup,
                IsAvailable = selectedSet.Contains(e.Id)
            })
            .ToList();

        cacheService.Set(cacheKey, response);

        return response;
    }

    public async Task<List<CoachEquipmentItemResponse>> GetCoachViewAsync(Guid userId)
    {
        var cacheKey = CacheKeys.UserAvailableEquipmentSelections(userId);

        if (cacheService.TryGetValue(cacheKey,
                out List<CoachEquipmentItemResponse>? cachedAvailableEquipmentSelections))
            return cachedAvailableEquipmentSelections;

        var user = await lookup.GetUserAsync(userId);
        var coach = await lookup.GetCoachAsync(user.CoachId!.Value);

        access.EnsureCurrentCoachOwnsUser(coach, user);

        var equipments = await equipmentRepository.GetActiveByCoachIdAsync(coach.Id);
        var selections = await equipmentSelectionRepository.GetByUserAsync(coach.Id, userId);
        var availableIds = selections.Select(s => s.EquipmentId).ToHashSet();

        var response = equipments
            .Where(e => availableIds.Contains(e.Id))
            .Select(e => new CoachEquipmentItemResponse
            {
                EquipmentId = e.Id,
                Name = e.Name,
                VideoUrl = e.VideoUrl,
                MuscleGroup = e.MuscleGroup
            })
            .ToList();

        cacheService.Set(cacheKey, response);

        return response;
    }

    public async Task<List<UserEquipmentItemResponse>> SaveAsync(Guid userId, SaveEquipmentSelectionsRequest request)
    {
        access.EnsureCurrentUserIs(userId);

        var user = await lookup.GetUserAsync(userId);
        var coach = await lookup.GetCoachAsync(user.CoachId!.Value);

        var distinctIds = request.AvailableEquipmentIds.Distinct().ToList();

        var allowedIds = (await equipmentRepository.GetActiveByCoachIdAsync(coach.Id))
            .Select(e => e.Id)
            .ToHashSet();

        var invalidIds = distinctIds.Where(id => !allowedIds.Contains(id)).ToList();

        if (invalidIds.Count > 0)
            throw new ValidationException("Existem equipamentos inválidos para este treinador.",
                invalidIds.Select(id => id.ToString()));

        var selections = distinctIds
            .Select(id => new EquipmentSelection(coach.Id, userId, id))
            .ToList();

        await equipmentSelectionRepository.SaveAsync(coach.Id, userId, selections);

        cacheService.RemoveMultiple(
            CacheKeys.UserEquipmentSelections(userId),
            CacheKeys.UserAvailableEquipmentSelections(userId));

        return await GetUserViewAsync(userId);
    }
}