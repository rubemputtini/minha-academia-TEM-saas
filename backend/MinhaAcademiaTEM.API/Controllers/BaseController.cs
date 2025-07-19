using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.API.Controllers;

public abstract class BaseController : ControllerBase
{
    protected Guid CurrentUserId =>
        Guid.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
            ? id
            : throw new UnauthorizedAccessException("Usuário não autenticado.");

    protected string CurrentUserEmail =>
        User.FindFirstValue(ClaimTypes.Email)
        ?? throw new UnauthorizedAccessException("Usuário não autenticado.");

    protected UserRole CurrentUserRole =>
        Enum.TryParse<UserRole>(User.FindFirstValue(ClaimTypes.Role), out var role)
            ? role
            : throw new UnauthorizedAccessException("Usuário não autenticado.");

    protected bool IsAdmin => CurrentUserRole == UserRole.Admin;
    protected bool IsCoach => CurrentUserRole == UserRole.Coach;
}