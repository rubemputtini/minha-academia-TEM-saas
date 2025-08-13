using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.Equipments;
using MinhaAcademiaTEM.Application.Services.Equipments;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/base-equipments")]
[Authorize]
public class BaseEquipmentsController(IBaseEquipmentService baseEquipmentService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await baseEquipmentService.GetAllAsync();

        return Ok(response);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var response = await baseEquipmentService.GetByIdAsync(id);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Create([FromBody] CreateBaseEquipmentRequest request)
    {
        var response = await baseEquipmentService.CreateAsync(request);

        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Update([FromRoute] Guid id,
        [FromBody] UpdateBaseEquipmentRequest request)
    {
        var response = await baseEquipmentService.UpdateAsync(id, request);

        return Ok(response);
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        await baseEquipmentService.DeleteAsync(id);

        return NoContent();
    }
}