using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/subscriptions")]
[Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
public class SubscriptionsController(
    ICurrentUserService currentUser,
    ISubscriptionAppService subscriptionAppService)
    : BaseController
{
    [HttpPost("cancel-at-period-end")]
    public async Task<IActionResult> CancelAtPeriodEnd()
    {
        var response = await subscriptionAppService.ScheduleCancelAtPeriodEndAsync(currentUser.GetUserId());

        return Ok(response);
    }

    [HttpPost("undo-scheduled-cancel")]
    public async Task<IActionResult> UndoCancel()
    {
        var response = await subscriptionAppService.UndoScheduledCancelAsync(currentUser.GetUserId());

        return Ok(response);
    }
}