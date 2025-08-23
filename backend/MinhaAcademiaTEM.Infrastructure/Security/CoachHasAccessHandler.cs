using Microsoft.AspNetCore.Authorization;
using MinhaAcademiaTEM.Application.Common;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Infrastructure.Security;

public class CoachHasAccessHandler(
    ICurrentUserService currentUser,
    EntityLookup lookup) : AuthorizationHandler<CoachHasAccessRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        CoachHasAccessRequirement requirement)
    {
        if (context.User.IsInRole(nameof(UserRole.Admin)))
        {
            context.Succeed(requirement);
            return;
        }

        if (!context.User.IsInRole(nameof(UserRole.Coach)))
            return;

        var coach = await lookup.GetCoachByUserIdAsync(currentUser.GetUserId());

        if (coach.HasAccess)
            context.Succeed(requirement);
        else
            context.Fail(new AuthorizationFailureReason(this,
                "Sua assinatura está inativa. Regularize o pagamento para reativar o acesso."));
    }
}