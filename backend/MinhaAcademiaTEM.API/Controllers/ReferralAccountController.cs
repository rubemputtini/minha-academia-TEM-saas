using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.ReferralAccount;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/referral-accounts")]
[Authorize]
public class ReferralAccountController(IReferralAccountService referralService) : BaseController
{
    [HttpGet("me")]
    [Authorize(Roles = nameof(UserRole.Coach))]
    public async Task<IActionResult> GetMyCoachReferral()
    {
        var response = await referralService.GetMyCoachReferralAsync();

        return Ok(response);
    }
}