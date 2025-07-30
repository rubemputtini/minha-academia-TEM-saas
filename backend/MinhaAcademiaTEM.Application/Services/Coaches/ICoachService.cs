using MinhaAcademiaTEM.Application.DTOs.Users;

namespace MinhaAcademiaTEM.Application.Services.Coaches;

public interface ICoachService
{
    Task<(IEnumerable<UserResponse> Clients, int TotalClients)> GetAllCoachClientsAsync(int page = 1, int pageSize = 10,
        string? searchTerm = null);
    Task<int> GetTotalClientsAsync();
    Task DeleteCoachClientAsync(Guid userId);
}