using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Auth;
using MinhaAcademiaTEM.Application.DTOs.Common;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Application.Services.Emails;
using MinhaAcademiaTEM.Application.Services.Helpers;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
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
    IGymRepository gymRepository,
    EntityLookup lookup,
    SlugGenerator slugGenerator,
    RoleManager<IdentityRole<Guid>> roleManager,
    IEmailService emailService,
    IConfiguration configuration,
    ICheckoutSessionReader sessionReader,
    IStripeReferralService referralService,
    ISubscriptionSummaryReader subscriptionSummaryReader)
    : IAuthService
{
    public async Task<LoginResponse> RegisterCoachAsync(CoachRegisterRequest request, bool sendWelcomeFreeEmail = true)
    {
        var existingUser = await userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new ValidationException("Já existe um usuário com esse e-mail.");

        var user = await CreateUserAsync(request.Name, request.Email, request.Password,
            phoneNumber: request.PhoneNumber);

        var adminEmail = configuration["AdminSettings:AdminEmail"];
        var isAdmin = string.Equals(request.Email, adminEmail, StringComparison.OrdinalIgnoreCase);

        var roleName = isAdmin ? nameof(UserRole.Admin) : nameof(UserRole.Coach);

        await EnsureRoleExistsAsync(roleName);
        await userManager.AddToRoleAsync(user, roleName);

        var address = new Address(
            request.Address.Street,
            request.Address.Number,
            request.Address.Complement,
            request.Address.Neighborhood,
            request.Address.City,
            request.Address.State,
            request.Address.Country,
            request.Address.PostalCode,
            request.Address.Latitude,
            request.Address.Longitude,
            user.Id
        );

        var slug = await slugGenerator.GenerateUniqueSlugAsync(request.Name);

        var coach = new Coach(
            user.Id,
            request.Name,
            request.Email,
            slug,
            address
        );

        await coachRepository.AddAsync(coach);

        await emailService.SendNewCoachEmailAsync(coach);

        if (sendWelcomeFreeEmail)
            await emailService.SendWelcomeFreeCoachEmailAsync(coach);

        var userRole = await GetUserRoleAsync(user);

        return GenerateLoginResponse(user, userRole);
    }

    public async Task<LoginResponse> RegisterCoachAfterPaymentAsync(CoachRegisterAfterPaymentRequest request)
    {
        var coachResponse = await sessionReader.GetPrefillAsync(request.SessionId);
        var plan = Enum.Parse<SubscriptionPlan>(coachResponse.SubscriptionPlan, true);

        var coachRequest = new CoachRegisterRequest
        {
            Name = coachResponse.Name,
            Email = coachResponse.Email,
            PhoneNumber = coachResponse.PhoneNumber,
            Password = request.Password,
            Address = new AddressRequest
            {
                Street = coachResponse.Address.Street,
                Number = coachResponse.Address.Number ?? string.Empty,
                Complement = coachResponse.Address.Complement,
                Neighborhood = coachResponse.Address.Neighborhood ?? string.Empty,
                City = coachResponse.Address.City,
                State = coachResponse.Address.State,
                Country = coachResponse.Address.Country,
                PostalCode = coachResponse.Address.PostalCode
            }
        };

        var response = await RegisterCoachAsync(coachRequest, false);

        var coach = await coachRepository.GetByUserIdAsync(response.UserId);

        if (coach == null) return response;

        coach.SetStripeData(coachResponse.StripeCustomerId, coachResponse.StripeSubscriptionId);
        coach.SetSubscription(plan, SubscriptionStatus.Active, coach.SubscriptionEndAt);

        await coachRepository.UpdateAsync(coach);
        await referralService.EnsurePromotionCodeForCoachAsync(coach.Id, coach.Slug);

        var summary = await subscriptionSummaryReader.FromSubscriptionIdAsync(coachResponse.StripeSubscriptionId);
        await emailService.SendSubscriptionConfirmedEmailAsync(coach, summary);

        return response;
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

        var user = await CreateUserAsync(request.Name, request.Email, request.Password, coachId: coach.Id);
        var gym = await CreateGymAsync(request, user.Id, coach.Id);

        await EnsureRoleExistsAsync(nameof(UserRole.User));
        await userManager.AddToRoleAsync(user, nameof(UserRole.User));

        var userRole = await GetUserRoleAsync(user);

        await emailService.SendNewClientEmailAsync(user, gym);

        return GenerateLoginResponse(user, userRole);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new UnauthorizedException("E-mail ou senha inválidos.");

        var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (result.IsLockedOut)
            throw new UnauthorizedException("A conta está temporariamente bloqueada.");

        if (!result.Succeeded)
            throw new UnauthorizedException("E-mail ou senha inválidos.");

        var role = await GetUserRoleAsync(user);

        if (role == UserRole.User)
        {
            var coach = await lookup.GetCoachAsync(user.CoachId!.Value);

            if (!coach.HasAccess)
                throw new ForbiddenException("Seu acesso foi bloqueado. Fale com o seu treinador.");
        }

        return GenerateLoginResponse(user, role);
    }

    public async Task<string> ForgotPasswordAsync(ForgotPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user != null)
        {
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            var resetLink =
                $"{configuration["AppSettings:FrontendUrl"]}/redefinir-senha?token={encodedToken}&email={request.Email}";

            var emailSent = await emailService.SendPasswordResetEmailAsync(user.Name, user.Email!, resetLink);

            if (!emailSent)
                throw new ValidationException("Não foi possível enviar o e-mail. Tente novamente.");
        }

        return "Se o e-mail for válido, enviaremos um link para redefinir a senha.";
    }

    public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new ValidationException("Não foi possível redefinir a senha.");

        var token = Uri.UnescapeDataString(request.Token);

        var result = await userManager.ResetPasswordAsync(user, token, request.NewPassword);

        if (!result.Succeeded)
            throw new ValidationException("Não foi possível redefinir a senha.",
                result.Errors.Select(e => e.Description));

        return "Senha redefinida com sucesso.";
    }

    private async Task<Gym> CreateGymAsync(UserRegisterRequest request, Guid userId, Guid coachId)
    {
        var gym = new Gym(coachId, request.GymName, request.GymCity, request.GymCountry, userId);

        await gymRepository.AddAsync(gym);

        return gym;
    }

    private async Task<User> CreateUserAsync(
        string name,
        string email,
        string password,
        Guid? coachId = null,
        string? phoneNumber = null)
    {
        var user = new User(name, email);

        if (coachId != null)
            user.AssignCoach(coachId.Value);

        if (phoneNumber != null)
            user.PhoneNumber = phoneNumber;

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