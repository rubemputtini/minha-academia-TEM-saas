using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MinhaAcademiaTEM.Application.Models;

namespace MinhaAcademiaTEM.API.Extensions;

public static class AuthenticationExtensions
{
    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtSection = configuration.GetSection("JwtSettings");
        var jwtSettings = jwtSection.Get<JwtSettings>();
        var key = Encoding.ASCII.GetBytes(jwtSettings!.SecretKey);

        services.Configure<JwtSettings>(jwtSection);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };
        });
    }
}