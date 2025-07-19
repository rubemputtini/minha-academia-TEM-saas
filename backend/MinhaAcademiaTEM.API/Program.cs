using MinhaAcademiaTEM.API.Extensions;
using MinhaAcademiaTEM.API.Middlewares;
using MinhaAcademiaTEM.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);

var app = builder.Build();

app.UseHttpsRedirection();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();