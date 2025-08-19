using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.DTOs.Billing;
using MinhaAcademiaTEM.Application.Services.Billing;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/billing-portal")]
[Authorize(Roles = $"{nameof(UserRole.Coach)},{nameof(UserRole.Admin)}")]
public class BillingPortalController(IBillingPortalService portalService) : BaseController
{
    [HttpPost("sessions")]
    public async Task<IActionResult> Create()
    {
        var url = await portalService.CreateCustomerPortalSessionAsync();

        return Ok(new PortalSessionResponse { Url = url });
    }
}