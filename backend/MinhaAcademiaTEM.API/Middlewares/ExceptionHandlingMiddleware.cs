using System.Net;
using System.Text.Json;
using MinhaAcademiaTEM.API.Models;
using MinhaAcademiaTEM.Domain.Exceptions;

namespace MinhaAcademiaTEM.API.Middlewares;

public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro na API | Path: {Path} | TraceId: {TraceId}", context.Request.Path,
                context.TraceIdentifier);

            var response = new ApiErrorResponse
            {
                Message = "Erro interno do servidor. Tente novamente.",
                Details = null
            };

            var statusCode = (int)HttpStatusCode.InternalServerError;

            switch (ex)
            {
                case ForbiddenException:
                    statusCode = (int)HttpStatusCode.Forbidden;
                    response.Message = ex.Message;
                    break;
                case NotFoundException:
                    statusCode = (int)HttpStatusCode.NotFound;
                    response.Message = ex.Message;
                    break;
                case UnauthorizedException:
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    response.Message = ex.Message;
                    break;
                case ValidationException validationEx:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    response.Message = validationEx.Message;
                    response.Details = string.Join("; ", validationEx.Errors);
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}