using MinhaAcademiaTEM.API.Extensions;
using MinhaAcademiaTEM.API.Middlewares;
using MinhaAcademiaTEM.Application.Extensions;
using MinhaAcademiaTEM.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApiServices(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (app.Environment.IsProduction())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRateLimiter();

app.UseResponseCompression();

app.Use(async (context, next) =>
{
    context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Append("X-Frame-Options", "DENY");
    context.Response.Headers.Append("X-XSS-Protection", "1; mode=block");

    await next();
});

app.UseCors("AllowSpecificOrigins");

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.MapHealthChecks("/health")
    .RequireRateLimiting("HealthPolicy");

app.Run();