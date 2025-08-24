using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Application.Services.Equipments;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/equipments")]
[Authorize]
public class EquipmentsController(IEquipmentService equipmentService) : BaseController
{
    [HttpGet("coaches/{coachId:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> GetByCoach(Guid coachId)
    {
        var response = await equipmentService.GetAllByCoachIdAsync(coachId);

        return Ok(response);
    }

    [HttpGet("coaches/{coachId:guid}/active")]
    public async Task<IActionResult> GetActiveByCoach(Guid coachId)
    {
        var response = await equipmentService.GetActiveByCoachIdAsync(coachId);

        return Ok(response);
    }

    [HttpGet("me")]
    [Authorize(Policy = "CoachHasAccess")]
    public async Task<IActionResult> GetAll()
    {
        var response = await equipmentService.GetAll();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await equipmentService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Policy = "CoachHasAccess")]
    public async Task<IActionResult> Create([FromBody] CreateEquipmentRequest request)
    {
        var response = await equipmentService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Policy = "CoachHasAccess")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEquipmentRequest request)
    {
        var response = await equipmentService.UpdateAsync(id, request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Policy = "CoachHasAccess")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await equipmentService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:guid}/active")]
    [Authorize(Policy = "CoachHasAccess")]
    public async Task<IActionResult> SetStatus(Guid id, [FromBody] ToggleEquipmentRequest request)
    {
        var response = await equipmentService.SetStatusAsync(id, request);

        return Ok(new { isActive = response });
    }
}