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
    [HttpGet("by-coach/{coachId:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> GetEquipmentsByCoach(Guid coachId)
    {
        var response = await equipmentService.GetAllByCoachIdAsync(coachId);

        return Ok(response);
    }

    [HttpGet("by-coach/{coachId:guid}/active")]
    public async Task<IActionResult> GetActiveEquipmentsByCoach(Guid coachId)
    {
        var response = await equipmentService.GetActiveByCoachIdAsync(coachId);

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetEquipment(Guid id)
    {
        var response = await equipmentService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> CreateEquipment([FromBody] CreateEquipmentRequest request)
    {
        var response = await equipmentService.CreateAsync(request);

        return CreatedAtAction(nameof(GetEquipment), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> UpdateEquipment(Guid id, [FromBody] UpdateEquipmentRequest request)
    {
        var response = await equipmentService.UpdateAsync(id, request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> DeleteEquipment(Guid id)
    {
        await equipmentService.DeleteAsync(id);

        return NoContent();
    }

    [HttpPatch("{id:guid}/toggle")]
    [Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> ToggleEquipment(Guid id, [FromBody] ToggleEquipmentRequest request)
    {
        var response = await equipmentService.ToggleActiveAsync(id, request);

        return Ok(new { isActive = response });
    }
}