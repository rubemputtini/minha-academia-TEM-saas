using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Account;
using MinhaAcademiaTEM.Application.DTOs.Common;
using MinhaAcademiaTEM.Application.Services.Account;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Account;

public class AccountServiceTests
{
    private readonly Mock<IUserRepository> _users = new();
    private readonly Mock<ICoachRepository> _coaches = new();
    private readonly Mock<IGymRepository> _gyms = new();
    private readonly Mock<ICurrentUserService> _current = new();
    private readonly Mock<UserManager<User>> _userManager = NewUserManager();

    private AccountService Service() => new(
        new EntityLookup(_users.Object, _coaches.Object, _gyms.Object),
        _current.Object,
        _coaches.Object,
        _gyms.Object,
        _userManager.Object
    );

    private static Mock<UserManager<User>> NewUserManager()
    {
        var store = new Mock<IUserStore<User>>();
        return new Mock<UserManager<User>>(
            store.Object, null, null, null, null, null, null, null, null
        );
    }

    [Fact]
    public async Task GetMyUserAsync_Should_Return_User_And_Gym_From_Lookup()
    {
        var userId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(userId);

        var user = TestData.User("Rubem", "rubem@test.com", id: userId);
        var gym = TestData.Gym(Guid.NewGuid(), "Academia X", "Porto", userId: userId);

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _gyms.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(gym);

        var service = Service();

        var result = await service.GetMyUserAsync();

        result.Name.Should().Be("Rubem");
        result.Email.Should().Be("rubem@test.com");
        result.GymName.Should().Be("Academia X");
        result.GymLocation.Should().Be("Porto");

        _users.Verify(r => r.GetByIdAsync(userId), Times.Once);
        _gyms.Verify(r => r.GetByUserIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task UpdateMyUserAsync_Should_Update_Gym_When_Only_Gym_Changed()
    {
        var userId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(userId);

        var user = TestData.User("Rubem", "rubem@test.com", id: userId);
        var gym = TestData.Gym(Guid.NewGuid(), "Old Gym", "Old City", userId: userId);

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _gyms.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(gym);
        _gyms.Setup(r => r.UpdateAsync(It.IsAny<Gym>())).Returns(Task.CompletedTask);

        var request = new UpdateMyUserRequest
        {
            Name = "Rubem",
            Email = "rubem@test.com",
            GymName = "New Gym",
            GymLocation = "New City"
        };

        var service = Service();

        var result = await service.UpdateMyUserAsync(request);

        result.GymName.Should().Be("New Gym");
        result.GymLocation.Should().Be("New City");

        _userManager.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        _gyms.Verify(r => r.UpdateAsync(It.Is<Gym>(g => g.Name == "New Gym" && g.Location == "New City")), Times.Once);
    }

    [Fact]
    public async Task UpdateMyUserAsync_Should_Update_Name_And_Save_User_When_Name_Changed()
    {
        var userId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(userId);

        var user = TestData.User("Old Name", "same@test.com", id: userId);
        var gym = TestData.Gym(Guid.NewGuid(), "Gym", "City", userId: userId);

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _gyms.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(gym);

        _userManager.Setup(m => m.UpdateAsync(user)).ReturnsAsync(IdentityResult.Success);

        var request = new UpdateMyUserRequest
        {
            Name = "New Name",
            Email = "same@test.com",
            GymName = gym.Name,
            GymLocation = gym.Location
        };

        var service = Service();

        var result = await service.UpdateMyUserAsync(request);

        result.Name.Should().Be("New Name");
        _userManager.Verify(m => m.UpdateAsync(user), Times.Once);
        _gyms.Verify(r => r.UpdateAsync(It.IsAny<Gym>()), Times.Never);
    }

    [Fact]
    public async Task UpdateMyUserAsync_Should_Throw_When_Email_Already_Exists()
    {
        var userId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(userId);

        var user = TestData.User("Old Name", "same@test.com", id: userId);
        var gym = TestData.Gym(Guid.NewGuid(), "Gym", "City", userId: userId);

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _gyms.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(gym);

        _userManager.Setup(m => m.FindByEmailAsync("new@test.com"))
            .ReturnsAsync(new User("Other", "new@test.com") { Id = Guid.NewGuid() });

        var request = new UpdateMyUserRequest
        {
            Name = "Rubem",
            Email = "new@test.com",
            GymName = gym.Name,
            GymLocation = gym.Location
        };

        var service = Service();

        var act = () => service.UpdateMyUserAsync(request);

        await act.Should().ThrowAsync<ValidationException>()
            .WithMessage("*Já existe um usuário com esse e-mail*");

        _userManager.Verify(m => m.SetEmailAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
        _userManager.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
    }

    [Fact]
    public async Task GetMyCoachAsync_Should_Map_Coach_To_Response()
    {
        var userId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(userId);

        var coach = TestData.Coach(name: "Coach X");
        var user = new User("Coach X", "coach@test.com") { Id = userId, PhoneNumber = "9999-0000" };
        coach.SetUser(user);

        _coaches.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(coach);

        var service = Service();

        var result = await service.GetMyCoachAsync();

        result.Name.Should().Be("Coach X");
        result.Email.Should().Be("coach@test.com");
        result.PhoneNumber.Should().Be("9999-0000");
        result.Address.City.Should().Be(coach.Address.City);
    }

    [Fact]
    public async Task UpdateMyCoachAsync_Should_Throw_When_SetPhone_Fails()
    {
        var userId = Guid.NewGuid();
        _current.Setup(c => c.GetUserId()).Returns(userId);

        var coach = TestData.Coach();
        var user = new User("Coach", coach.Email) { Id = userId, PhoneNumber = "1111" };
        coach.SetUser(user);

        _users.Setup(r => r.GetByIdAsync(userId)).ReturnsAsync(user);
        _coaches.Setup(r => r.GetByUserIdAsync(userId)).ReturnsAsync(coach);

        _userManager.Setup(m => m.SetPhoneNumberAsync(user, "2222"))
            .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "bad phone" }));

        var req = new UpdateMyCoachRequest
        {
            Name = coach.Name,
            Email = coach.Email,
            PhoneNumber = "2222",
            Address = new AddressRequest
            {
                Street = coach.Address.Street,
                Number = coach.Address.Number,
                Complement = coach.Address.Complement,
                Neighborhood = coach.Address.Neighborhood,
                City = coach.Address.City,
                State = coach.Address.State,
                Country = coach.Address.Country,
                PostalCode = coach.Address.PostalCode
            }
        };

        var service = Service();

        var act = () => service.UpdateMyCoachAsync(req);

        await act.Should().ThrowAsync<ValidationException>();

        _userManager.Verify(m => m.UpdateAsync(It.IsAny<User>()), Times.Never);
        _coaches.Verify(r => r.UpdateAsync(It.IsAny<Coach>()), Times.Never);
    }
}