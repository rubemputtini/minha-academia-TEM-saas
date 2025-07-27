using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.Coaches;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/coach")]
[Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
public class CoachController(ICoachService coachService) : BaseController
{
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (clients, totalClients) = await coachService.GetAllCoachClientsAsync(page, pageSize, searchTerm);

        return Ok(new { clients, totalClients });
    }

    [HttpDelete("users/{userId:guid}")]
    public async Task<IActionResult> DeleteUser(Guid userId)
    {
        await coachService.DeleteCoachClientAsync(userId);

        return NoContent();
    }
}