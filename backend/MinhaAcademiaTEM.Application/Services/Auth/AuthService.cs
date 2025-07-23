using Microsoft.AspNetCore.Identity;
using MinhaAcademiaTEM.Application.DTOs.Auth;
using MinhaAcademiaTEM.Domain.Constants;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.Domain.Exceptions;

namespace MinhaAcademiaTEM.Application.Services.Auth;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    ITokenService tokenService,
    IUserRepository userRepository,
    ICoachRepository coachRepository,
    SlugGenerator slugGenerator,
    RoleManager<IdentityRole<Guid>> roleManager)
    : IAuthService
{
    public async Task<LoginResponse> RegisterCoachAsync(CoachRegisterRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new ValidationException("Já existe um usuário com esse e-mail.");

        var user = await CreateUserAsync(request.Name, request.Email, request.Password,
            phoneNumber: request.PhoneNumber);

        await EnsureRoleExistsAsync(nameof(UserRole.Coach));
        await userManager.AddToRoleAsync(user, nameof(UserRole.Coach));

        var address = new Address
        {
            Street = request.Street,
            Number = request.Number,
            Complement = request.Complement,
            Neighborhood = request.Neighborhood,
            City = request.City,
            State = request.State,
            Country = request.Country,
            PostalCode = request.PostalCode,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        var slug = await slugGenerator.GenerateUniqueSlugAsync(request.Name);

        var coach = new Coach
        {
            Id = user.Id,
            Name = request.Name,
            Email = request.Email,
            Slug = slug,
            Address = address,
            IsActive = true,
            SubscriptionPlan = SubscriptionPlan.Trial,
            SubscriptionStatus = SubscriptionStatus.Trial
        };

        await coachRepository.AddAsync(coach);

        return GenerateLoginResponse(user, UserRole.Coach);
    }

    public async Task<LoginResponse> RegisterUserAsync(UserRegisterRequest request)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new ValidationException("Já existe um usuário com esse e-mail.");

        var coach = await coachRepository.GetBySlugAsync(request.CoachCode);

        if (coach == null)
            throw new ValidationException("Código do treinador inválido ou não encontrado.");

        var users = await userRepository.GetAllByCoachIdAsync(coach.Id);
        var maxUsers = SubscriptionPlanLimits.GetMaxUsers(coach.SubscriptionPlan);

        if (users.Count >= maxUsers)
            throw new ValidationException("O plano atual do treinador atingiu o limite de alunos permitidos.");

        var user = await CreateUserAsync(request.Name, request.Email, request.Password, coach.Id);

        await EnsureRoleExistsAsync(nameof(UserRole.User));
        await userManager.AddToRoleAsync(user, nameof(UserRole.User));

        return GenerateLoginResponse(user, UserRole.User);
    }

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

        return GenerateLoginResponse(user, role);
    }

    private async Task<User> CreateUserAsync(
        string name,
        string email,
        string password,
        Guid? coachId = null,
        string? phoneNumber = null)
    {
        var user = new User
        {
            Name = name,
            UserName = email,
            Email = email,
            CoachId = coachId,
            PhoneNumber = phoneNumber
        };

        var result = await userManager.CreateAsync(user, password);

        if (!result.Succeeded)
            throw new ValidationException("Erro ao criar o usuário.", result.Errors.Select(e => e.Description));

        return user;
    }

    private async Task EnsureRoleExistsAsync(string roleName)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
            await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
    }

    private LoginResponse GenerateLoginResponse(User user, UserRole role)
    {
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