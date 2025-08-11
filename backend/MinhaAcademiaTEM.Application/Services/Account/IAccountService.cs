using MinhaAcademiaTEM.Application.DTOs.Account;

namespace MinhaAcademiaTEM.Application.Services.Account;

public interface IAccountService
{
    Task<MyUserResponse> GetMyUserAsync();
    Task<MyUserResponse> UpdateMyUserAsync(UpdateMyUserRequest request);
    Task<MyCoachResponse> GetMyCoachAsync();
    Task<MyCoachResponse> UpdateMyCoachAsync(UpdateMyCoachRequest request);
}