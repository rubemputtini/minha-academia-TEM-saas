using System.Text;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.Webhooks;
using Stripe;

namespace MinhaAcademiaTEM.API.Controllers.Webhooks;

[ApiController]
[Route("api/v1/webhooks/stripe")]
public class StripeWebhookController(IStripeWebhookService webhookService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var payload = await new StreamReader(HttpContext.Request.Body, Encoding.UTF8).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        try
        {
            await webhookService.HandleAsync(payload, signature);
            return Ok();
        }
        catch (StripeException)
        {
            return BadRequest();
        }
    }
}