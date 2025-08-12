using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Application.Common;

public sealed class AccessChecks(ICurrentUserService currentUser)
{
    public void EnsureCurrentUserIs(Guid expectedUserId)
    {
        if (currentUser.GetUserId() != expectedUserId)
            throw new UnauthorizedException("Você não tem permissão para executar esta operação.");
    }

    public void EnsureCurrentCoachOwnsUser(Coach coach, User user)
    {
        if (!user.CoachId.HasValue || user.CoachId.Value != coach.Id)
            throw new ForbiddenException("O usuário não pertence a este treinador.");

        if (currentUser.GetUserId() != coach.User!.Id)
            throw new ForbiddenException("Você não tem permissão para visualizar este aluno.");
    }
}