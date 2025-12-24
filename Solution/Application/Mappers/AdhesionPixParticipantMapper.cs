using Application.DTOs.Responses;
using Application.Interfaces.Mappers;
using Domain.Entities;

namespace Application.Mappers;

public sealed class AdhesionPixParticipantMapper : IMapper<AdhesionPixParticipant, PixAdhesionParticipantDto>
{
    public PixAdhesionParticipantDto Map(AdhesionPixParticipant source)
    {
        return new PixAdhesionParticipantDto(
            source.Order,
            source.ShortName,
            source.Ispb,
            source.Cnpj,
            source.InstitutionType,
            source.BcbAuthorized,
            source.SpiParticipation,
            source.PixParticipation,
            source.PixMode,
            source.AdhesionStatus
        );
    }
}
