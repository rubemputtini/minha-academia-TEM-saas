using MinhaAcademiaTEM.Application.DTOs.Auth;

namespace MinhaAcademiaTEM.Application.Services.Auth;

public interface IAuthService
{
    Task<LoginResponse> RegisterCoachAsync(CoachRegisterRequest request, bool sendWelcomeFreeEmail = true);
    Task<LoginResponse> RegisterCoachAfterPaymentAsync(CoachRegisterAfterPaymentRequest request);
    Task<LoginResponse> RegisterUserAsync(UserRegisterRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<string> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<string> ResetPasswordAsync(ResetPasswordRequest request);
}