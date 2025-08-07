using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Equipments;

public class BaseEquipmentService(IBaseEquipmentRepository baseEquipmentRepository, IAppCacheService cacheService)
    : IBaseEquipmentService
{
    public async Task<List<BaseEquipmentResponse>> GetAllAsync()
    {
        var cacheKey = CacheKeys.AllBaseEquipments;

        if (cacheService.TryGetValue(cacheKey, out List<BaseEquipmentResponse>? cachedBaseEquipments))
            return cachedBaseEquipments;

        var baseEquipments = await baseEquipmentRepository.GetAllAsync();

        var responses = baseEquipments.Select(be => new BaseEquipmentResponse
        {
            Id = be.Id,
            Name = be.Name,
            PhotoUrl = be.PhotoUrl,
            VideoUrl = be.VideoUrl,
            MuscleGroup = be.MuscleGroup
        }).ToList();

        cacheService.Set(cacheKey, responses);

        return responses;
    }

    public async Task<BaseEquipmentResponse> GetByIdAsync(Guid id)
    {
        var baseEquipment = await baseEquipmentRepository.GetByIdAsync(id);

        if (baseEquipment == null)
            throw new NotFoundException("Equipamento base não encontrado.");

        var response = new BaseEquipmentResponse
        {
            Id = baseEquipment.Id,
            Name = baseEquipment.Name,
            PhotoUrl = baseEquipment.PhotoUrl,
            VideoUrl = baseEquipment.VideoUrl,
            MuscleGroup = baseEquipment.MuscleGroup
        };

        return response;
    }

    public async Task<BaseEquipmentResponse> CreateAsync(CreateBaseEquipmentRequest request)
    {
        var baseEquipment = new BaseEquipment
        {
            Name = request.Name,
            PhotoUrl = request.PhotoUrl,
            VideoUrl = request.VideoUrl,
            MuscleGroup = request.MuscleGroup
        };

        await baseEquipmentRepository.AddAsync(baseEquipment);

        cacheService.Remove(CacheKeys.AllBaseEquipments);

        var response = new BaseEquipmentResponse
        {
            Id = baseEquipment.Id,
            Name = baseEquipment.Name,
            PhotoUrl = baseEquipment.PhotoUrl,
            VideoUrl = baseEquipment.VideoUrl,
            MuscleGroup = baseEquipment.MuscleGroup
        };

        return response;
    }

    public async Task<BaseEquipmentResponse> UpdateAsync(Guid id, UpdateBaseEquipmentRequest request)
    {
        var baseEquipment = await baseEquipmentRepository.GetByIdAsync(id);

        if (baseEquipment == null)
            throw new NotFoundException("Equipamento base não encontrado.");

        baseEquipment.Name = request.Name;
        baseEquipment.PhotoUrl = request.PhotoUrl;
        baseEquipment.VideoUrl = request.VideoUrl;
        baseEquipment.MuscleGroup = request.MuscleGroup;

        await baseEquipmentRepository.UpdateAsync(baseEquipment);

        cacheService.Remove(CacheKeys.AllBaseEquipments);

        var response = new BaseEquipmentResponse
        {
            Id = baseEquipment.Id,
            Name = baseEquipment.Name,
            PhotoUrl = baseEquipment.PhotoUrl,
            VideoUrl = baseEquipment.VideoUrl,
            MuscleGroup = baseEquipment.MuscleGroup
        };

        return response;
    }

    public async Task DeleteAsync(Guid id)
    {
        var baseEquipment = await baseEquipmentRepository.GetByIdAsync(id);

        if (baseEquipment == null)
            throw new NotFoundException("Equipamento base não encontrado.");

        await baseEquipmentRepository.DeleteAsync(baseEquipment);
        
        cacheService.Remove(CacheKeys.AllBaseEquipments);
    }
}