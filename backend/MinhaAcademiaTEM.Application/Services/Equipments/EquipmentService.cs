using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Equipments;

public class EquipmentService(
    IEquipmentRepository equipmentRepository,
    IAppCacheService cacheService,
    ICurrentUserService currentUser,
    EntityLookup lookup,
    AccessChecks access,
    IPlanRulesService planRulesService)
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
            PhotoUrl = e.BaseEquipment.PhotoUrl,
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
        var coach = await lookup.GetCoachAsync(coachId);
        var user = await lookup.GetUserAsync(currentUser.GetUserId());

        access.EnsureCurrentUserHasPermission(user, coach);

        var cacheKey = CacheKeys.CoachActiveEquipments(coachId);

        if (cacheService.TryGetValue(cacheKey, out List<EquipmentResponse>? cachedCoachActiveEquipments))
            return cachedCoachActiveEquipments;

        var equipments = await equipmentRepository.GetActiveByCoachIdAsync(coachId);

        var responses = equipments.Select(e => new EquipmentResponse
        {
            Id = e.Id,
            Name = e.Name,
            PhotoUrl = e.BaseEquipment.PhotoUrl,
            VideoUrl = e.VideoUrl,
            MuscleGroup = e.MuscleGroup,
            BaseEquipmentId = e.BaseEquipmentId,
            IsActive = e.IsActive
        }).ToList();

        cacheService.Set(cacheKey, responses);

        return responses;
    }

    public async Task<List<EquipmentResponse>> GetAll()
    {
        var coachId = currentUser.GetUserId();

        return await GetAllByCoachIdAsync(coachId);
    }

    public async Task<EquipmentResponse> GetByIdAsync(Guid id)
    {
        var equipment = await GetEquipmentAsync(id);
        var user = await lookup.GetUserAsync(currentUser.GetUserId());

        access.EnsureCanView(equipment, user);

        var response = new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            PhotoUrl = equipment.BaseEquipment.PhotoUrl,
            VideoUrl = equipment.VideoUrl,
            MuscleGroup = equipment.MuscleGroup,
            BaseEquipmentId = equipment.BaseEquipmentId,
            IsActive = equipment.IsActive
        };

        return response;
    }

    public async Task<EquipmentResponse> CreateAsync(CreateEquipmentRequest request)
    {
        await planRulesService.EnsureCapabilityAsync(currentUser.GetUserId(), Capability.ManageCustomEquipment);

        var equipment = new Equipment(
            request.Name,
            request.VideoUrl,
            request.MuscleGroup,
            request.BaseEquipmentId,
            currentUser.GetUserId()
        );

        await equipmentRepository.AddAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);

        var response = new EquipmentResponse
        {
            Id = equipment.Id,
            Name = equipment.Name,
            PhotoUrl = equipment.BaseEquipment.PhotoUrl,
            VideoUrl = equipment.VideoUrl,
            MuscleGroup = equipment.MuscleGroup,
            BaseEquipmentId = equipment.BaseEquipmentId,
            IsActive = equipment.IsActive
        };

        return response;
    }

    public async Task<EquipmentResponse> UpdateAsync(Guid id, UpdateEquipmentRequest request)
    {
        await planRulesService.EnsureCapabilityAsync(currentUser.GetUserId(), Capability.ManageCustomEquipment);

        var equipment = await GetEquipmentAsync(id);
        access.EnsureCurrentCoachOwns(equipment);

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
            PhotoUrl = equipment.BaseEquipment.PhotoUrl,
            VideoUrl = equipment.VideoUrl,
            MuscleGroup = equipment.MuscleGroup,
            BaseEquipmentId = equipment.BaseEquipmentId,
            IsActive = equipment.IsActive
        };

        return response;
    }

    public async Task DeleteAsync(Guid id)
    {
        await planRulesService.EnsureCapabilityAsync(currentUser.GetUserId(), Capability.ManageCustomEquipment);

        var equipment = await GetEquipmentAsync(id);
        access.EnsureCurrentCoachOwns(equipment);

        await equipmentRepository.DeleteAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);
    }

    public async Task<bool> SetStatusAsync(Guid id, ToggleEquipmentRequest request)
    {
        await planRulesService.EnsureCapabilityAsync(currentUser.GetUserId(), Capability.ModifyEquipmentStatus);

        var equipment = await GetEquipmentAsync(id);
        access.EnsureCurrentCoachOwns(equipment);

        equipment.SetActive(request.IsActive);

        await equipmentRepository.UpdateAsync(equipment);

        InvalidateCoachEquipments(equipment.CoachId);

        return equipment.IsActive;
    }

    private async Task<Equipment> GetEquipmentAsync(Guid id)
    {
        var equipment = await equipmentRepository.GetByIdWithBaseAsync(id);

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