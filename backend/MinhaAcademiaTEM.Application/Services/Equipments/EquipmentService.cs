using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Equipments;

public class EquipmentService(IEquipmentRepository equipmentRepository, IAppCacheService cacheService)
    : IEquipmentService
{
    public async Task<List<EquipmentResponse>> GetAllByCoachIdAsync(Guid coachId)
    {
        var cacheKey = CacheKeys.CoachEquipments(coachId);

        if (cacheService.TryGetValue(cacheKey, out List<EquipmentResponse>? cachedCoachEquipments))
            return cachedCoachEquipments;

        var equipments = await equipmentRepository.GetAllByCoachIdAsync(coachId);

        var responses = equipments.Select(e => new EquipmentResponse
        {
            Id = e.Id,
            Name = e.Name,
            VideoUrl = e.VideoUrl,
            MuscleGroup = e.MuscleGroup,
            BaseEquipmentId = e.BaseEquipmentId,
            IsActive = e.IsActive
        }).ToList();

        cacheService.Set(cacheKey, responses);

        return responses;
    }

    public async Task<List<EquipmentResponse>> GetActiveByCoachIdAsync(Guid coachId)
    {
        var cacheKey = CacheKeys.CoachActiveEquipments(coachId);

        if (cacheService.TryGetValue(cacheKey, out List<EquipmentResponse>? cachedCoachActiveEquipments))
            return cachedCoachActiveEquipments;

        var equipments = await equipmentRepository.GetActiveByCoachIdAsync(coachId);

        var responses = equipments.Select(e => new EquipmentResponse
        {
            Id = e.Id,
            Name = e.Name,
            VideoUrl = e.VideoUrl,
            MuscleGroup = e.MuscleGroup,
            BaseEquipmentId = e.BaseEquipmentId,
            IsActive = e.IsActive
        }).ToList();

        cacheService.Set(cacheKey, responses);

        return responses;
    }

    public async Task<EquipmentResponse> GetByIdAsync(Guid id)
    {
        var equipment = await GetEquipmentAsync(id);

        var response = new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            VideoUrl = equipment.VideoUrl,
            MuscleGroup = equipment.MuscleGroup,
            BaseEquipmentId = equipment.BaseEquipmentId,
            IsActive = equipment.IsActive
        };

        return response;
    }

    public async Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request)
    {
        var equipment = new Equipment(
            request.Name,
            request.VideoUrl,
            request.MuscleGroup,
            request.BaseEquipmentId,
            request.CoachId
        );

        await equipmentRepository.AddAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);

        var response = new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            VideoUrl = equipment.VideoUrl,
            MuscleGroup = equipment.MuscleGroup,
            BaseEquipmentId = equipment.BaseEquipmentId,
            IsActive = equipment.IsActive
        };

        return response;
    }

    public async Task<EquipmentResponse> UpdateAsync(Guid id, UpdateEquipmentRequest request)
    {
        var equipment = await GetEquipmentAsync(id);

        var hasChanged =
            equipment.Name != request.Name ||
            equipment.VideoUrl != request.VideoUrl ||
            equipment.MuscleGroup != request.MuscleGroup;

        if (hasChanged)
            equipment.UpdateInfo(request.Name, request.VideoUrl, request.MuscleGroup!.Value);

        await equipmentRepository.UpdateAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);

        var response = new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            VideoUrl = equipment.VideoUrl,
            MuscleGroup = equipment.MuscleGroup,
            BaseEquipmentId = equipment.BaseEquipmentId,
            IsActive = equipment.IsActive
        };

        return response;
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await GetEquipmentAsync(id);

        await equipmentRepository.DeleteAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);
    }

    public async Task<bool> SetStatusAsync(Guid id, ToggleEquipmentRequest request)
    {
        var equipment = await GetEquipmentAsync(id);

        equipment.SetActive(request.IsActive);

        await equipmentRepository.UpdateAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);

        return equipment.IsActive;
    }

    private async Task<Equipment> GetEquipmentAsync(Guid id)
    {
        var equipment = await equipmentRepository.GetByIdAsync(id);

        if (equipment == null)
            throw new NotFoundException("Equipamento não encontrado.");

        return equipment;
    }

    private void InvalidateCoachEquipments(Guid coachId)
    {
        cacheService.RemoveMultiple(
            CacheKeys.CoachEquipments(coachId),
            CacheKeys.CoachActiveEquipments(coachId));
    }
}