using MinhaAcademiaTEM.Application.DTOs.Auth;

namespace MinhaAcademiaTEM.Application.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginRequest request);
}