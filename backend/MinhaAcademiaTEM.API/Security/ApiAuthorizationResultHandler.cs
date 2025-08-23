using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using MinhaAcademiaTEM.API.Models;

namespace MinhaAcademiaTEM.API.Security;

public sealed class ApiAuthorizationResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _middleware = new();

    public async Task HandleAsync(RequestDelegate next, HttpContext context, AuthorizationPolicy policy,
        PolicyAuthorizationResult authorizeResult)
    {
        if (authorizeResult.Succeeded)
        {
            await _middleware.HandleAsync(next, context, policy, authorizeResult);
            return;
        }

        context.Response.ContentType = "application/json";

        if (authorizeResult.Challenged)
        {
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            context.Response.Headers.WWWAuthenticate = "Bearer";

            await context.Response.WriteAsJsonAsync(new ApiErrorResponse
            {
                Message = "Não autenticado.",
                Details = null
            });

            return;
        }

        context.Response.StatusCode = (int)HttpStatusCode.Forbidden;

        var reason = authorizeResult.AuthorizationFailure?
            .FailureReasons?.FirstOrDefault()?.Message;

        await context.Response.WriteAsJsonAsync(new ApiErrorResponse
        {
            Message = string.IsNullOrWhiteSpace(reason) ? "Acesso bloqueado." : reason,
            Details = null
        });
    }
}