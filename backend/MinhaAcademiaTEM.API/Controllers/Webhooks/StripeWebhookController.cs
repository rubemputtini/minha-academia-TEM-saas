using System.Text;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Application.Services.Webhooks;
using Stripe;

namespace MinhaAcademiaTEM.API.Controllers.Webhooks;

[ApiController]
[Route("api/v1/webhooks/stripe")]
public class StripeWebhookController(
    IStripeWebhookService webhookService,
    ILogger<StripeWebhookController> logger) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Index()
    {
        var payload = await new StreamReader(HttpContext.Request.Body, Encoding.UTF8).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        try
        {
            await webhookService.HandleAsync(payload, signature);
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Erro Stripe no processamento do webhook.");
        }

        return Ok();
    }
}