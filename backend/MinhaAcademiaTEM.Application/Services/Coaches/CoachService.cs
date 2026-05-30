using MinhaAcademiaTEM.Application.Caching;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Training;
using MinhaAcademiaTEM.Application.DTOs.Users;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Coaches;

public class CoachService(
    ICurrentUserService currentUserService,
    IAppCacheService cacheService,
    IUserRepository userRepository,
    EntityLookup lookup,
    IPlanRulesService planRules)
    : ICoachService
{
    public async Task<(IEnumerable<UserResponse> Clients, int TotalClients)> GetAllCoachClientsAsync(
        Guid coachId, int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        page = Math.Max(1, page);
        pageSize = Math.Clamp(pageSize, 1, 50);

        var isSearch = !string.IsNullOrWhiteSpace(searchTerm);
        var cacheKey = CacheKeys.CoachClientsPaged(coachId, page, pageSize);

        if (!isSearch && cacheService.TryGetValue(cacheKey,
                out (IEnumerable<UserResponse> Clients, int TotalClients) cachedClients))
            return cachedClients;

        var totalClients = await userRepository.CountByCoachAsync(coachId, searchTerm);
        var skip = (page - 1) * pageSize;

        var clients = await userRepository.SearchByCoachAsync(coachId, searchTerm, skip, pageSize);

        var responses = clients.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email!,
            CoachId = u.CoachId,
            CoachName = u.Coach!.Name,
            IsActive = u.IsActive,
            NextTrainingChangeAt = u.NextTrainingChangeAt
        });

        var result = (responses, totalClients);

        if (!isSearch)
            cacheService.Set(cacheKey, result);

        return result;
    }

    public async Task<(IEnumerable<UserResponse> Clients, int TotalClients)> GetOwnClientsAsync(
        int page = 1, int pageSize = 10, string? searchTerm = null)
    {
        var coachId = currentUserService.GetUserId();

        var result = await GetAllCoachClientsAsync(coachId, page, pageSize, searchTerm);

        return result;
    }

    public async Task<int> GetTotalClientsAsync()
    {
        var coachId = currentUserService.GetUserId();

        return await userRepository.CountByCoachAsync(coachId);
    }

    public async Task DeleteCoachClientAsync(Guid userId)
    {
        var coachId = currentUserService.GetUserId();
        var user = await lookup.GetUserAsync(userId);

        if (user.CoachId != coachId)
            throw new NotFoundException("Cliente não encontrado ou não pertence a este treinador.");

        await userRepository.DeleteAsync(user);

        InvalidateClientCache(coachId);
        InvalidateUserCache(userId);
    }

    public async Task<List<TrainingScheduleItemResponse>> GetTrainingScheduleAsync()
    {
        var coachId = currentUserService.GetUserId();
        var cacheKey = CacheKeys.CoachTrainingSchedule(coachId);

        if (cacheService.TryGetValue(cacheKey, out List<TrainingScheduleItemResponse>? cached))
            return cached!;

        var users = await userRepository.GetTrainingScheduleByCoachIdAsync(coachId);

        var result = users.Select(u => new TrainingScheduleItemResponse
        {
            UserId = u.Id,
            Name = u.Name,
            GymName = u.Gym?.Name ?? string.Empty,
            NextTrainingChangeAt = u.NextTrainingChangeAt
        }).ToList();

        cacheService.Set(cacheKey, result);

        return result;
    }

    public async Task UpdateClientTrainingDateAsync(Guid userId, DateTime? nextTrainingChangeAt)
    {
        var coachId = currentUserService.GetUserId();
        var user = await lookup.GetUserAsync(userId);

        if (user.CoachId != coachId)
            throw new NotFoundException("Cliente não encontrado ou não pertence a este treinador.");

        user.SetNextTrainingChangeAt(nextTrainingChangeAt);
        await userRepository.UpdateAsync(user);

        InvalidateClientCache(coachId);
    }

    public async Task SetClientActiveAsync(Guid userId, bool isActive)
    {
        var coachId = currentUserService.GetUserId();

        await planRules.EnsureCapabilityAsync(coachId, Capability.ManageClientActiveStatus);

        var user = await lookup.GetUserAsync(userId);

        if (user.CoachId != coachId)
            throw new ForbiddenException("Este aluno não pertence ao seu cadastro.");

        user.SetActive(isActive);
        await userRepository.UpdateAsync(user);

        InvalidateClientCache(coachId);
    }

    private void InvalidateClientCache(Guid coachId)
    {
        cacheService.RemoveByPrefix(CacheKeys.CoachClients(coachId));
        cacheService.Remove(CacheKeys.CoachTrainingSchedule(coachId));
    }

    private void InvalidateUserCache(Guid userId)
    {
        cacheService.Remove(CacheKeys.UserEquipmentSelections(userId));
        cacheService.Remove(CacheKeys.UserAvailableEquipmentSelections(userId));
        cacheService.Remove(CacheKeys.UserEquipmentNotes(userId));
    }
}