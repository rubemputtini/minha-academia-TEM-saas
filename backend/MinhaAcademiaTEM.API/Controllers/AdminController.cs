using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.Admin;
using MinhaAcademiaTEM.Application.Services.Admins;
using MinhaAcademiaTEM.Application.Services.Subscriptions;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController(
    IAdminService adminService,
    IAdminSubscriptionService adminSubscriptionService) : BaseController
{
    [HttpGet("coaches")]
    public async Task<IActionResult> GetCoaches(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (coaches, totalCoaches) = await adminService.GetAllCoachesAsync(page, pageSize, searchTerm);

        return Ok(new { coaches, totalCoaches });
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (users, totalUsers) = await adminService.GetAllUsersAsync(page, pageSize, searchTerm);

        return Ok(new { users, totalUsers });
    }

    [HttpGet("coaches/total")]
    public async Task<IActionResult> GetTotalCoaches()
    {
        var response = await adminService.GetTotalCoachesAsync();

        return Ok(response);
    }

    [HttpGet("users/total")]
    public async Task<IActionResult> GetTotalUsers()
    {
        var response = await adminService.GetTotalUsersAsync();

        return Ok(response);
    }

    [HttpDelete("users/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await adminService.DeleteUserAsync(userId);

        return NoContent();
    }

    [HttpPut("coaches/{coachId:guid}/subscription")]
    public async Task<IActionResult> UpdateCoachSubscription(
        Guid coachId, [FromBody] UpdateCoachSubscriptionRequest request)
    {
        var response = await adminService.UpdateCoachSubscriptionAsync(coachId, request);

        return Ok(response);
    }

    [HttpPost("coaches/{coachId:guid}/subscription/cancel")]
    public async Task<IActionResult> CancelNow([FromRoute] Guid coachId)
    {
        var response = await adminSubscriptionService.CancelNowAsync(coachId);

        return Ok(response);
    }
}