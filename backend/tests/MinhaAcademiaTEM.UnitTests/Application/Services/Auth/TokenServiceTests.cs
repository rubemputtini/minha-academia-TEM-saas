using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MinhaAcademiaTEM.Application.Models;
using MinhaAcademiaTEM.Application.Services.Auth;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.UnitTests.Application.Helpers;

namespace MinhaAcademiaTEM.UnitTests.Application.Services.Auth;

public class TokenServiceTests
{
    private const string CorrectKey = "correct-key-32bytes-minimum-ABCDEFG123456";
    private const string WrongKey = "wrong-key-also-32bytes-minimum-XYZ7890";

    private static IOptions<JwtSettings> Options(
        string key = CorrectKey,
        string issuer = "test-issuer",
        string audience = "test-audience",
        int hours = 1)
        => new OptionsWrapper<JwtSettings>(new JwtSettings
        {
            SecretKey = key,
            Issuer = issuer,
            Audience = audience,
            ExpirationInHours = hours
        });

    private static string? GetClaim(JwtSecurityToken token, params string[] types)
        => token.Claims.FirstOrDefault(c => types.Contains(c.Type))?.Value;

    [Fact]
    public void GenerateToken_Should_Embed_Basic_Claims()
    {
        var svc = new TokenService(Options());
        var user = TestData.User();

        var jwt = svc.GenerateToken(user, UserRole.Coach);
        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        GetClaim(token, ClaimTypes.NameIdentifier, "nameid", "sub")
            .Should().Be(user.Id.ToString());

        GetClaim(token, ClaimTypes.Name, "unique_name", "name")
            .Should().Be(user.Name);

        GetClaim(token, ClaimTypes.Email, "email")
            .Should().Be(user.Email);

        GetClaim(token, ClaimTypes.Role, "role", "roles")
            .Should().Be(nameof(UserRole.Coach));

        token.Issuer.Should().Be("test-issuer");
        token.Audiences.Should().Contain("test-audience");
    }

    [Fact]
    public void GenerateToken_Should_Validate_With_Configured_Key_Issuer_Audience()
    {
        var opts = Options();
        var svc = new TokenService(opts);
        var user = TestData.User();

        var jwt = svc.GenerateToken(user, UserRole.User);

        var parameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = opts.Value.Issuer,
            ValidateAudience = true,
            ValidAudience = opts.Value.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(opts.Value.SecretKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };

        var handler = new JwtSecurityTokenHandler();
        handler.Invoking(h => h.ValidateToken(jwt, parameters, out _))
            .Should().NotThrow();
    }

    [Fact]
    public void GenerateToken_Should_Set_Expiration_From_Config()
    {
        const int hours = 3;
        var opts = Options(hours: hours);
        var svc = new TokenService(opts);
        var user = TestData.User();

        var before = DateTime.UtcNow;
        var jwt = svc.GenerateToken(user, UserRole.User);
        var after = DateTime.UtcNow;

        var token = new JwtSecurityTokenHandler().ReadJwtToken(jwt);

        token.ValidTo.Should().BeOnOrAfter(before.AddHours(hours).AddSeconds(-5));
        token.ValidTo.Should().BeOnOrBefore(after.AddHours(hours).AddSeconds(5));
    }

    [Fact]
    public void GenerateToken_Should_Fail_With_Wrong_Key()
    {
        var svc = new TokenService(Options(key: CorrectKey));
        var user = TestData.User();
        var jwt = svc.GenerateToken(user, UserRole.User);

        var wrongParams = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(WrongKey)),
            ValidateLifetime = false
        };

        var handler = new JwtSecurityTokenHandler();

        handler.Invoking(h => h.ValidateToken(jwt, wrongParams, out _))
            .Should().Throw<SecurityTokenInvalidSignatureException>();
    }
}