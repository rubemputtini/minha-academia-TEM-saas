using MinhaAcademiaTEM.Domain.Entities;

namespace MinhaAcademiaTEM.Domain.Interfaces;

public interface ICurrentUserService
{
    Guid GetUserId();
    string GetUserEmail();
    UserRole GetUserRole();
}