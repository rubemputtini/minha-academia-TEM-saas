using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.ExchangeRates;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/exchange-rates")]
[Authorize]
public class ExchangeRatesController(IExchangeRateService exchangeRateService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetRates([FromQuery] string from, [FromQuery] string to)
    {
        if (string.IsNullOrWhiteSpace(from) || string.IsNullOrWhiteSpace(to))
            return BadRequest("Parâmetros 'from' e 'to' são obrigatórios.");

        var rates = await exchangeRateService.GetRatesAsync(from, to);

        if (rates == null)
            return StatusCode(502, "Não foi possível obter as cotações de câmbio.");

        return Ok(new { rates });
    }
}