using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.Admins;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = nameof(UserRole.Admin))]
public class AdminController(IAdminService adminService) : BaseController
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (users, totalUsers) = await adminService.GetAllUsersAsync(page, pageSize, searchTerm);

        return Ok(new { users, totalUsers });
    }

    [HttpGet("coaches")]
    public async Task<IActionResult> GetCoaches(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (coaches, totalCoaches) = await adminService.GetAllCoachesAsync(page, pageSize, searchTerm);

        return Ok(new { coaches, totalCoaches });
    }
}