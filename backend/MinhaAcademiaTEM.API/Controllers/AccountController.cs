using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.Account;
using MinhaAcademiaTEM.Application.Services.Account;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/account")]
[Authorize]
public class AccountController(IAccountService accountService) : BaseController
{
    [HttpGet("users/me")]
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> GetMyUser()
    {
        var response = await accountService.GetMyUserAsync();

        return Ok(response);
    }

    [HttpPut("users/me")]
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> UpdateMyUser(UpdateMyUserRequest request)
    {
        var response = await accountService.UpdateMyUserAsync(request);

        return Ok(response);
    }

    [HttpGet("coaches/me")]
    [Authorize(Roles = nameof(UserRole.Coach))]
    public async Task<IActionResult> GetMyCoach()
    {
        var response = await accountService.GetMyCoachAsync();

        return Ok(response);
    }

    [HttpPut("coaches/me")]
    [Authorize(Roles = nameof(UserRole.Coach))]
    public async Task<IActionResult> UpdateMyCoach(UpdateMyCoachRequest request)
    {
        var response = await accountService.UpdateMyCoachAsync(request);

        return Ok(response);
    }
}