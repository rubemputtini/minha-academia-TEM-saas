using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MinhaAcademiaTEM.Domain.Entities;
using MinhaAcademiaTEM.Domain.Exceptions;
using MinhaAcademiaTEM.Domain.Interfaces;

namespace MinhaAcademiaTEM.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor accessor) : ICurrentUserService
{
    private readonly ClaimsPrincipal? _user = accessor.HttpContext?.User;

    public Guid GetUserId()
    {
        var id = _user?.FindFirstValue(ClaimTypes.NameIdentifier);

        return Guid.TryParse(id, out var guid)
            ? guid
            : throw new UnauthorizedException("Usuário não autenticado.");
    }

    public string GetUserEmail() =>
        _user?.FindFirstValue(ClaimTypes.Email)
        ?? throw new UnauthorizedException("Usuário não autenticado.");

    public UserRole GetUserRole()
    {
        var role = _user?.FindFirstValue(ClaimTypes.Role);

        return Enum.TryParse<UserRole>(role, out var parsedRole)
            ? parsedRole
            : throw new UnauthorizedException("Usuário não autenticado.");
    }
}