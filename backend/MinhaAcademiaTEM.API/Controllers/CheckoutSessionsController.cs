using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.API.Extensions;
using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/checkout-sessions")]
public class CheckoutSessionsController(ICheckoutSessionsService checkoutService, ICheckoutSessionReader sessionReader)
    : BaseController
{
    [AllowAnonymous]
    [HttpPost("signup")]
    public async Task<IActionResult> CreateSignup([FromBody] SignupCheckoutRequest request)
    {
        var key = Request.GetIdempotencyKeyOrNew();
        var url = await checkoutService.CreateSignupAsync(request.SubscriptionPlan, key);

        return Ok(new CheckoutSessionResponse { Url = url });
    }

    [Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
    [HttpPost("upgrade")]
    public async Task<IActionResult> CreateCoachSubscription([FromBody] UpgradeCheckoutRequest request)
    {
        var key = Request.GetIdempotencyKeyOrNew();
        var url = await checkoutService.CreateCoachSubscriptionAsync(request.SubscriptionPlan, key);

        return Ok(new CheckoutSessionResponse { Url = url });
    }

    [AllowAnonymous]
    [HttpGet("{sessionId}/prefill")]
    public async Task<IActionResult> GetPrefill([FromRoute] string sessionId)
    {
        var response = await sessionReader.GetPrefillAsync(sessionId);

        return Ok(response);
    }
}