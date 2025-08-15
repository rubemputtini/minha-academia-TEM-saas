using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.EquipmentNotes;
using MinhaAcademiaTEM.Application.Services.EquipmentNotes;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/equipment-notes")]
[Authorize]
public class EquipmentNotesController(
    IEquipmentNoteService equipmentNoteService)
    : BaseController
{
    [HttpGet("user/{userId:guid}")]
    [Authorize(Roles = nameof(UserRole.Coach))]
    public async Task<IActionResult> GetByUserId([FromRoute] Guid userId)
    {
        var response = await equipmentNoteService.GetByUserIdAsync(userId);

        return Ok(response);
    }

    [HttpPut("me")]
    [Authorize(Roles = nameof(UserRole.User))]
    public async Task<IActionResult> Upsert([FromBody] UpsertEquipmentNoteRequest request)
    {
        var response = await equipmentNoteService.UpsertAsync(request);

        return Ok(response);
    }
}