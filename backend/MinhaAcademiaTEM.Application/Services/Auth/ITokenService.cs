using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Application.Services.Auth;

public interface ITokenService
{
    string GenerateToken(User user, UserRole userRole);
}