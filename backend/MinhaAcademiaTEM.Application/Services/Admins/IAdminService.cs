using MinhaAcademiaTEM.Application.DTOs.Coaches;
using MinhaAcademiaTEM.Application.DTOs.Users;

namespace MinhaAcademiaTEM.Application.Services.Admins;

public interface IAdminService
{
    Task<(IEnumerable<CoachResponse> Coaches, int TotalCoaches)> GetAllCoachesAsync(int page = 1, int pageSize = 10,
        string? searchTerm = null);

    Task<(IEnumerable<UserResponse> Users, int TotalUsers)> GetAllUsersAsync(int page = 1, int pageSize = 10,
        string? searchTerm = null);

    Task<int> GetTotalCoachesAsync();
    Task<int> GetTotalUsersAsync();
    Task DeleteUserAsync(Guid userId);
}