using Microsoft.AspNetCore.Identity;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Application.DTOs.Account;
using MinhaAcademiaTEM.Application.DTOs.Common;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Services.Account;

public class AccountService(
    EntityLookup lookup,
    ICurrentUserService currentUserService,
    ICoachRepository coachRepository,
    IGymRepository gymRepository,
    UserManager<User> userManager) : IAccountService
{
    public async Task<MyUserResponse> GetMyUserAsync()
    {
        var userId = currentUserService.GetUserId();
        var user = await lookup.GetUserAsync(userId);
        var gym = await lookup.GetGymByUserIdAsync(userId);

        var response = new MyUserResponse()
        {
            Name = user.Name,
            Email = user.Email!,
            GymName = gym.Name,
            GymLocation = gym.Location
        };

        return response;
    }

    public async Task<MyUserResponse> UpdateMyUserAsync(UpdateMyUserRequest request)
    {
        var userId = currentUserService.GetUserId();
        var user = await lookup.GetUserAsync(userId);
        var gym = await lookup.GetGymByUserIdAsync(userId);

        var userChanged = await ApplyUserChanges(user, request.Name, request.Email);

        if (userChanged)
            await SaveUserAsync(user, "Falha ao salvar alterações do usuário.");

        var gymChanged =
            !string.Equals(request.GymName, gym.Name, StringComparison.Ordinal) ||
            !string.Equals(request.GymLocation, gym.Location, StringComparison.Ordinal);

        if (gymChanged)
        {
            gym.UpdateInfo(request.GymName, request.GymLocation);
            await gymRepository.UpdateAsync(gym);
        }

        var response = new MyUserResponse
        {
            Name = user.Name,
            Email = user.Email!,
            GymLocation = gym.Location,
            GymName = gym.Name
        };

        return response;
    }

    public async Task<MyCoachResponse> GetMyCoachAsync()
    {
        var userId = currentUserService.GetUserId();
        var coach = await lookup.GetCoachAsync(userId);

        var response = MapToCoachResponse(coach);

        return response;
    }

    public async Task<MyCoachResponse> UpdateMyCoachAsync(UpdateMyCoachRequest request)
    {
        var userId = currentUserService.GetUserId();
        var user = await lookup.GetUserAsync(userId);
        var coach = await lookup.GetCoachAsync(user.CoachId!.Value);

        var userChanged = false;

        if (!string.Equals(request.Name, user.Name, StringComparison.Ordinal))
        {
            coach.UpdateName(request.Name);

            userChanged = true;
        }

        if (!string.Equals(request.Email, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            await UpdateUserEmailAsync(user, request.Email);
            coach.UpdateEmail(request.Email);

            userChanged = true;
        }

        if (!string.Equals(request.PhoneNumber, coach.User!.PhoneNumber, StringComparison.Ordinal))
        {
            var setPhone = await userManager.SetPhoneNumberAsync(user, request.PhoneNumber);

            if (!setPhone.Succeeded)
                throw new ValidationException("Não foi possível atualizar o telefone.",
                    setPhone.Errors.Select(e => e.Description));

            userChanged = true;
        }

        if (userChanged)
            await SaveUserAsync(user, "Falha ao salvar alterações do treinador.");

        if (!AddressEquals(request.Address, coach.Address))
        {
            coach.Address.UpdateAddress(
                request.Address.Street,
                request.Address.Number,
                request.Address.Complement,
                request.Address.Neighborhood,
                request.Address.City,
                request.Address.State,
                request.Address.Country,
                request.Address.PostalCode,
                request.Address.Latitude,
                request.Address.Longitude
            );
        }

        await coachRepository.UpdateAsync(coach);

        var response = MapToCoachResponse(coach);

        return response;
    }

    private async Task<bool> ApplyUserChanges(User user, string newName, string newEmail)
    {
        var changed = false;

        if (!string.Equals(newName, user.Name, StringComparison.Ordinal))
        {
            user.UpdateName(newName);
            changed = true;
        }

        if (!string.Equals(newEmail, user.Email, StringComparison.OrdinalIgnoreCase))
        {
            await UpdateUserEmailAsync(user, newEmail);
            changed = true;
        }

        return changed;
    }

    private async Task SaveUserAsync(User user, string errorMessage)
    {
        var update = await userManager.UpdateAsync(user);

        if (!update.Succeeded)
            throw new ValidationException(errorMessage, update.Errors.Select(e => e.Description));
    }

    private async Task UpdateUserEmailAsync(User user, string newEmail)
    {
        var existingUser = await userManager.FindByEmailAsync(newEmail);

        if (existingUser != null && existingUser.Id != user.Id)
            throw new ValidationException("Já existe um usuário com esse e-mail.");

        var setEmail = await userManager.SetEmailAsync(user, newEmail);

        if (!setEmail.Succeeded)
            throw new ValidationException("Não foi possível atualizar o e-mail.",
                setEmail.Errors.Select(e => e.Description));

        var setUserName = await userManager.SetUserNameAsync(user, newEmail);

        if (!setUserName.Succeeded)
            throw new ValidationException("Não foi possível atualizar o nome do usuário.",
                setUserName.Errors.Select(e => e.Description));
    }

    private static bool AddressEquals(AddressRequest a, Address b)
    {
        return string.Equals(a.Street?.Trim(), b.Street, StringComparison.Ordinal) &&
               string.Equals(a.Number?.Trim(), b.Number, StringComparison.Ordinal) &&
               string.Equals(a.Complement?.Trim() ?? string.Empty, b.Complement, StringComparison.Ordinal) &&
               string.Equals(a.Neighborhood?.Trim(), b.Neighborhood, StringComparison.Ordinal) &&
               string.Equals(a.City?.Trim(), b.City, StringComparison.Ordinal) &&
               string.Equals(a.State?.Trim(), b.State, StringComparison.Ordinal) &&
               string.Equals(a.Country?.Trim(), b.Country, StringComparison.Ordinal) &&
               string.Equals(a.PostalCode?.Trim(), b.PostalCode, StringComparison.Ordinal);
    }

    private static MyCoachResponse MapToCoachResponse(Coach coach)
    {
        return new MyCoachResponse
        {
            Name = coach.Name,
            Email = coach.User!.Email!,
            PhoneNumber = coach.User!.PhoneNumber!,
            SubscriptionStatus = coach.SubscriptionStatus.ToString(),
            SubscriptionPlan = coach.SubscriptionPlan.ToString(),
            SubscriptionEndAt = coach.SubscriptionEndAt,
            Address = new AddressResponse
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
    }
}