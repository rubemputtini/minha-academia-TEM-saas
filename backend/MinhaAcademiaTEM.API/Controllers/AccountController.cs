using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using MinhaAcademiaTEM.Application.DTOs.Auth;
using MinhaAcademiaTEM.Application.Services.Auth;

namespace MinhaAcademiaTEM.API.Controllers;

[ApiController]
[Route("api/v1/account")]
public class AccountController(IAuthService authService) : BaseController
{
    [HttpPost("register/coach")]
    public async Task<IActionResult> RegisterCoach([FromBody] CoachRegisterRequest request)
    {
        var response = await authService.RegisterCoachAsync(request);

        return Ok(response);
    }

    [HttpPost("register/user")]
    public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequest request)
    {
        var response = await authService.RegisterUserAsync(request);

        return Ok(response);
    }

    [EnableRateLimiting("LoginLimiter")]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await authService.LoginAsync(request);

        return Ok(response);
    }
}