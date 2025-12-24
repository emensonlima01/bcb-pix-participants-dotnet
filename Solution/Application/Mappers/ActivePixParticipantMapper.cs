using Application.DTOs.Responses;
using Application.Interfaces.Mappers;
using Domain.Entities;

namespace Application.Mappers;

public sealed class ActivePixParticipantMapper : IMapper<ActivePixParticipant, PixActiveParticipantDto>
{
    public PixActiveParticipantDto Map(ActivePixParticipant source)
    {
        return new PixActiveParticipantDto(
            source.Order,
            source.ShortName,
            source.Ispb,
            source.Cnpj,
            source.InstitutionType,
            source.BcbAuthorized,
            source.SpiParticipation,
            source.PixParticipation,
            source.PixMode,
            source.PaymentInitiation,
            source.CashoutFacilitator
        );
    }
}
