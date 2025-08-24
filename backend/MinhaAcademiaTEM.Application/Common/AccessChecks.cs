using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Common;

public sealed class AccessChecks(ICurrentUserService currentUser)
{
    public void EnsureCurrentUserIs(Guid expectedUserId)
    {
        if (currentUser.GetUserId() != expectedUserId)
            throw new ForbiddenException("Você não tem permissão para executar esta operação.");
    }

    public void EnsureCurrentCoachOwnsUser(Coach coach, User user)
    {
        EnsureCurrentUserIs(coach.Id);

        if (!user.CoachId.HasValue || user.CoachId.Value != coach.Id)
            throw new ForbiddenException("O usuário não pertence a este treinador.");
    }

    public void EnsureCurrentUserHasPermission(User user, Coach coach)
    {
        var currentId = currentUser.GetUserId();

        var isCoach = currentId == coach.Id;
        var isClientOfCoach = user.CoachId == coach.Id;

        if (!(isCoach || isClientOfCoach))
            throw new ForbiddenException("Você não tem permissão para visualizar este treinador.");
    }

    public void EnsureCanView(Equipment equipment, User user)
    {
        var currentId = currentUser.GetUserId();

        var isCoachOwner = currentId == equipment.CoachId;
        var isClientOfCoachOwner = user.CoachId == equipment.CoachId;

        if (!(isCoachOwner || isClientOfCoachOwner))
            throw new ForbiddenException("Você não tem permissão para visualizar este recurso.");
    }

    public void EnsureCurrentCoachOwns(Equipment equipment)
    {
        if (currentUser.GetUserId() != equipment.CoachId)
            throw new ForbiddenException("Você não tem permissão para alterar este equipamento.");
    }
}