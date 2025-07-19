using Microsoft.AspNetCore.Identity;
using MinhaAcademiaTEM.Application.DTOs.Auth;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Services.Auth;

public class AuthService(UserManager<User> userManager, SignInManager<User> signInManager, ITokenService tokenService)
    : IAuthService
{
    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (result.IsLockedOut)
            throw new UnauthorizedAccessException("A conta está temporariamente bloqueada.");

        if (!result.Succeeded)
            throw new UnauthorizedAccessException("E-mail ou senha inválidos.");

        var role = await GetUserRoleAsync(user);
        var token = tokenService.GenerateToken(user, role);

        return new LoginResponse
        {
            UserId = user.Id,
            Name = user.Name,
            Email = user.Email!,
            Role = role.ToString(),
            Token = token
        };
    }

    private async Task<UserRole> GetUserRoleAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault();

        if (Enum.TryParse<UserRole>(role, out var parsed))
            return parsed;

        return UserRole.User;
    }
}