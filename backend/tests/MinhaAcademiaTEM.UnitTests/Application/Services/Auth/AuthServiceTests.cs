using FluentAssertions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Auth;
using MinhaAcademiaTEM.Application.DTOs.Common;
using MinhaAcademiaTEM.Application.Services.Auth;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Application.Services.Emails;
using MinhaAcademiaTEM.Application.Services.Helpers;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Auth;

public class AuthServiceTests
{
    private readonly Mock<UserManager<User>> _userManager = NewUserManager();
    private readonly FakeSignInManager _signInManager;
    private readonly Mock<ITokenService> _tokenService = new();
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly SlugGenerator _slugGenerator;
    private readonly Mock<RoleManager<IdentityRole<Guid>>> _roleManager = NewRoleManager();
    private readonly Mock<IEmailService> _email = new();
    private readonly Mock<IConfiguration> _config = new();
    private readonly Mock<ICheckoutSessionReader> _checkout = new();
    private readonly Mock<IStripeReferralService> _referral = new();
    private readonly Mock<ISubscriptionSummaryReader> _summaryReader = new();

    public AuthServiceTests()
    {
        var http = new Mock<IHttpContextAccessor>();
        var claims = new Mock<IUserClaimsPrincipalFactory<User>>();
        var options = Options.Create(new IdentityOptions());
        var logger = new Mock<ILogger<SignInManager<User>>>();
        var schemes = new Mock<IAuthenticationSchemeProvider>();

        _signInManager = new FakeSignInManager(
            _userManager.Object,
            http.Object,
            claims.Object,
            options,
            logger.Object,
            schemes.Object
        );

        _slugGenerator = new SlugGenerator(_coaches.Object);
        _coaches.Setup(r => r.ExistsSlugAsync(It.IsAny<string>())).ReturnsAsync(false);

        _config.SetupGet(c => c["AdminSettings:AdminEmail"]).Returns("admin@site.com");
        _config.SetupGet(c => c["AppSettings:FrontendUrl"]).Returns("https://app");

        _tokenService.Setup(t => t.GenerateToken(It.IsAny<User>(), It.IsAny<UserRole>()))
            .Returns("JWT");
        
        _userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string>());
    }

    private AuthService Service() => new(
        _userManager.Object,
        _signInManager,
        _tokenService.Object,
        _users.Object,
        _coaches.Object,
        _gyms.Object,
        new EntityLookup(_users.Object, _coaches.Object, _gyms.Object),
        _slugGenerator,
        _roleManager.Object,
        _email.Object,
        _config.Object,
        _checkout.Object,
        _referral.Object,
        _summaryReader.Object
    );

    private static Mock<UserManager<User>> NewUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null
        );
    }

    private static Mock<RoleManager<IdentityRole<Guid>>> NewRoleManager()
    {
        var store = new Mock<IRoleStore<IdentityRole<Guid>>>();
        return new Mock<RoleManager<IdentityRole<Guid>>>(
            store.Object, null, null, null, null
        );
    }

    [Fact]
    public async Task RegisterCoachAsync_Should_Create_User_And_Coach_Send_Emails_And_Return_Token()
    {
        var req = new CoachRegisterRequest
        {
            Name = "Rubem",
            Email = "rubem@test.com",
            Password = "Password1!",
            PhoneNumber = "9999-0000",
            Address = new AddressRequest
            {
                Street = "Rua", Number = "1", Neighborhood = "Centro",
                City = "Cidade", State = "PT", Country = "PT", PostalCode = "0000-000"
            }
        };

        _userManager.Setup(m => m.FindByEmailAsync(req.Email)).ReturnsAsync((User)null!);
        _userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), req.Password)).ReturnsAsync(IdentityResult.Success);
        _roleManager.Setup(r => r.RoleExistsAsync(nameof(UserRole.Coach))).ReturnsAsync(false);
        _roleManager.Setup(r => r.CreateAsync(It.IsAny<IdentityRole<Guid>>())).ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), nameof(UserRole.Coach)))
            .ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { nameof(UserRole.Coach) });
        _email.Setup(e => e.SendNewCoachEmailAsync(It.IsAny<Coach>())).ReturnsAsync(true);
        _email.Setup(e => e.SendWelcomeFreeCoachEmailAsync(It.IsAny<Coach>())).ReturnsAsync(true);
        _coaches.Setup(r => r.AddAsync(It.IsAny<Coach>())).Returns(Task.CompletedTask);

        var service = Service();

        var res = await service.RegisterCoachAsync(req);

        res.Name.Should().Be("Rubem");
        res.Email.Should().Be("rubem@test.com");
        res.Token.Should().Be("JWT");

        _coaches.Verify(r => r.AddAsync(It.IsAny<Coach>()), Times.Once);
        _email.Verify(e => e.SendNewCoachEmailAsync(It.IsAny<Coach>()), Times.Once);
        _email.Verify(e => e.SendWelcomeFreeCoachEmailAsync(It.IsAny<Coach>()), Times.Once);
    }

    [Fact]
    public async Task RegisterCoachAsync_Should_Throw_When_Email_Already_Exists()
    {
        var req = new CoachRegisterRequest
        {
            Name = "X", Email = "dup@t.com", Password = "Pass1!", Address = new AddressRequest()
        };
        _userManager.Setup(m => m.FindByEmailAsync(req.Email)).ReturnsAsync(new User("X", "dup@t.com"));

        var service = Service();

        var act = () => service.RegisterCoachAsync(req);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RegisterUserAsync_Should_Throw_When_CoachCode_Invalid()
    {
        var req = new UserRegisterRequest
        {
            Name = "U", Email = "u@t.com", Password = "Pass1!",
            CoachCode = "nope", GymName = "G", GymLocation = "L"
        };
        _userManager.Setup(m => m.FindByEmailAsync(req.Email)).ReturnsAsync((User)null!);
        _coaches.Setup(r => r.GetBySlugAsync(req.CoachCode)).ReturnsAsync((Coach?)null);

        var service = Service();
        var act = () => service.RegisterUserAsync(req);

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task RegisterUserAsync_Should_Create_User_Gym_Assign_Role_And_Return_Token()
    {
        var coach = TestData.Coach(slug: "abc");
        _coaches.Setup(r => r.GetBySlugAsync("abc")).ReturnsAsync(coach);
        _users.Setup(r => r.GetAllByCoachIdAsync(coach.Id)).ReturnsAsync(new List<User>());
        _userManager.Setup(m => m.FindByEmailAsync("u@t.com")).ReturnsAsync((User)null!);
        _userManager.Setup(m => m.CreateAsync(It.IsAny<User>(), "Pass1!")).ReturnsAsync(IdentityResult.Success);
        _roleManager.Setup(r => r.RoleExistsAsync(nameof(UserRole.User))).ReturnsAsync(true);
        _userManager.Setup(m => m.AddToRoleAsync(It.IsAny<User>(), nameof(UserRole.User)))
            .ReturnsAsync(IdentityResult.Success);
        _userManager.Setup(m => m.GetRolesAsync(It.IsAny<User>()))
            .ReturnsAsync(new List<string> { nameof(UserRole.User) });
        _gyms.Setup(r => r.AddAsync(It.IsAny<Gym>())).Returns(Task.CompletedTask);
        _email.Setup(e => e.SendNewClientEmailAsync(It.IsAny<User>(), It.IsAny<Gym>())).ReturnsAsync(true);

        var req = new UserRegisterRequest
        {
            Name = "User",
            Email = "u@t.com",
            Password = "Pass1!",
            CoachCode = "abc",
            GymName = "Gym",
            GymLocation = "City"
        };

        var service = Service();

        var res = await service.RegisterUserAsync(req);

        res.Email.Should().Be("u@t.com");
        res.Token.Should().Be("JWT");

        _gyms.Verify(r => r.AddAsync(It.IsAny<Gym>()), Times.Once);
        _email.Verify(e => e.SendNewClientEmailAsync(It.IsAny<User>(), It.IsAny<Gym>()), Times.Once);
    }
    
    [Fact]
    public async Task LoginAsync_Should_Throw_When_User_Not_Found()
    {
        var req = new LoginRequest { Email = "x@t.com", Password = "pw" };
        _userManager.Setup(m => m.FindByEmailAsync(req.Email)).ReturnsAsync((User)null!);

        var service = Service();
        var act = () => service.LoginAsync(req);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task LoginAsync_Should_Throw_Forbidden_When_User_Role_And_Coach_Has_No_Access()
    {
        var user = new User("U", "u@t.com") { Id = Guid.NewGuid() };
        user.AssignCoach(Guid.NewGuid());

        _userManager.Setup(m => m.FindByEmailAsync("u@t.com")).ReturnsAsync(user);
        _signInManager.NextResult = SignInResult.Success; 
        _userManager.Setup(m => m.GetRolesAsync(user))
            .ReturnsAsync(new List<string> { nameof(UserRole.User) });

        var coach = TestData.Coach(id: user.CoachId!.Value);
        coach.SetCanceled();
        _coaches.Setup(r => r.GetByIdAsync(coach.Id)).ReturnsAsync(coach);

        var service = Service();
        var act = () => service.LoginAsync(new LoginRequest { Email = "u@t.com", Password = "pw" });

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task ForgotPasswordAsync_Should_Throw_When_Email_Service_Fails()
    {
        var user = new User("U", "u@t.com");
        _userManager.Setup(m => m.FindByEmailAsync("u@t.com")).ReturnsAsync(user);
        _userManager.Setup(m => m.GeneratePasswordResetTokenAsync(user)).ReturnsAsync("token-123");
        _email.Setup(e => e.SendPasswordResetEmailAsync("U", "u@t.com", It.IsAny<string>()))
            .ReturnsAsync(false);

        var service = Service();
        var act = () => service.ForgotPasswordAsync(new ForgotPasswordRequest { Email = "u@t.com" });

        await act.Should().ThrowAsync<ValidationException>();
    }

    [Fact]
    public async Task ResetPasswordAsync_Should_Return_Success_When_Token_Valid()
    {
        var user = new User("U", "u@t.com");
        _userManager.Setup(m => m.FindByEmailAsync("u@t.com")).ReturnsAsync(user);
        _userManager.Setup(m => m.ResetPasswordAsync(user, "token", "NewPass1!"))
            .ReturnsAsync(IdentityResult.Success);

        var service = Service();

        var res = await service.ResetPasswordAsync(new ResetPasswordRequest
        {
            Email = "u@t.com",
            Token = "token",
            NewPassword = "NewPass1!"
        });

        res.Should().Be("Senha redefinida com sucesso.");
    }

    private sealed class FakeSignInManager(
        UserManager<User> userManager,
        IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<User> claimsFactory,
        IOptions<IdentityOptions> optionsAccessor,
        ILogger<SignInManager<User>> logger,
        IAuthenticationSchemeProvider schemes)
        : SignInManager<User>(userManager, contextAccessor, claimsFactory, optionsAccessor, logger, schemes)
    {
        public SignInResult NextResult { get; set; } = SignInResult.Success;

        public override Task<SignInResult> CheckPasswordSignInAsync(User user, string password, bool lockoutOnFailure)
            => Task.FromResult(NextResult);
    }
}