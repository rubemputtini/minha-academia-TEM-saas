using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/geo")]
public class GeoController : BaseController
{
    [HttpGet("currency")]
    [AllowAnonymous]
    public IActionResult GetCurrency()
    {
        var country = Request.Headers["CF-IPCountry"].FirstOrDefault()?.ToUpperInvariant();

        var currency = country switch
        {
            "BR" => "BRL",
            "US" => "USD",
            _ => "EUR"
        };

        Response.Headers.Append("Vary", "CF-IPCountry");

        return Ok(new { currency });
    }
}