using MinhaAcademiaTEM.Domain.Constants;

namespace MinhaAcademiaTEM.Application.Services.Subscriptions;

public interface IPlanRulesService
{
    Task EnsureCapabilityAsync(Guid coachId, Capability capability);
}