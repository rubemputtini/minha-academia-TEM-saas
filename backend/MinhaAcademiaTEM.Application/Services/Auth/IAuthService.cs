using MinhaAcademiaTEM.Application.DTOs.Auth;

namespace MinhaAcademiaTEM.Application.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> RegisterCoachAsync(CoachRegisterRequest request);
    Task<LoginResponse> RegisterUserAsync(UserRegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
}