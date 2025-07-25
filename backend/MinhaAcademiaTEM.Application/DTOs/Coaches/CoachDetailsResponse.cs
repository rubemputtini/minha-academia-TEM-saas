using MinhaAcademiaTEM.Application.DTOs.Common;

namespace MinhaAcademiaTEM.Application.DTOs.Coaches;

public class CoachDetailsResponse : CoachResponse
{
    public AddressResponse Address { get; set; } = null!;
}