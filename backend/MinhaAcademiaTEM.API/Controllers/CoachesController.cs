using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.Coaches;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/coaches")]
[Authorize(Policy = "CoachHasAccess")]
public class CoachesController(ICoachService coachService) : BaseController
{
    [Authorize(Roles = nameof(UserRole.Admin))]
    [HttpGet("{coachId:guid}/clients")]
    public async Task<IActionResult> GetClientsByCoach(
        Guid coachId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (clients, totalClients) = await coachService.GetAllCoachClientsAsync(coachId, page, pageSize, searchTerm);

        return Ok(new { clients, totalClients });
    }

    [HttpGet("me/clients")]
    public async Task<IActionResult> GetOwnClients(
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
    {
        var (clients, totalClients) = await coachService.GetOwnClientsAsync(page, pageSize, searchTerm);

        return Ok(new { clients, totalClients });
    }

    [HttpGet("me/clients/total")]
    public async Task<IActionResult> GetTotalClients()
    {
        var response = await coachService.GetTotalClientsAsync();

        return Ok(response);
    }

    [HttpDelete("me/clients/{userId:guid}")]
    public async Task<IActionResult> DeleteClient(Guid userId)
    {
        await coachService.DeleteCoachClientAsync(userId);

        return NoContent();
    }
}