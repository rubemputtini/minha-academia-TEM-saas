using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.EquipmentSelections;
using MinhaAcademiaTEM.Application.Services.EquipmentSelections;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/equipment-selections")]
public class EquipmentSelectionsController(
    IEquipmentSelectionService equipmentSelectionService,
    ICurrentUserService currentUserService) : BaseController
{
    [Authorize(Roles = nameof(UserRole.User))]
    [HttpGet("me")]
    public async Task<IActionResult> GetUserView()
    {
        var response = await equipmentSelectionService.GetUserViewAsync(currentUserService.GetUserId());

        return Ok(response);
    }

    [Authorize(Policy = "CoachHasAccess")]
    [HttpGet("users/{userId:guid}")]
    public async Task<IActionResult> GetCoachView(Guid userId)
    {
        var response = await equipmentSelectionService.GetCoachViewAsync(userId);

        return Ok(response);
    }

    [Authorize(Roles = nameof(UserRole.User))]
    [HttpPut("me")]
    public async Task<IActionResult> SaveOwn([FromBody] SaveEquipmentSelectionsRequest request)
    {
        var response = await equipmentSelectionService.SaveOwnAsync(currentUserService.GetUserId(), request);

        return Ok(response);
    }

    [Authorize(Policy = "CoachHasAccess")]
    [HttpPut("users/{userId:guid}")]
    public async Task<IActionResult> SaveClient(Guid userId, [FromBody] SaveEquipmentSelectionsRequest request)
    {
        var response = await equipmentSelectionService.SaveClientAsync(userId, request);
        
        return Ok(response);
    }
}