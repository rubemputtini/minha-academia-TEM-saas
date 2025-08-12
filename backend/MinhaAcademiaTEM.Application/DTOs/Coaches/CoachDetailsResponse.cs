using MinhaAcademiaTEM.Application.DTOs.Common;

namespace MinhaAcademiaTEM.Application.DTOs.Coaches;

public sealed class CoachDetailsResponse : CoachResponse
{
    public AddressResponse Address { get; init; } = null!;
}