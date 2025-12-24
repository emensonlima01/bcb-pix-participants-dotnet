using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.UseCases;

public sealed class GetPixParticipantsUseCase(
    IPixParticipantsRepository repository,
    IMapper<ActivePixParticipant, PixActiveParticipantDto> activeMapper,
    IMapper<AdhesionPixParticipant, PixAdhesionParticipantDto> adhesionMapper) 
    : IUseCase<PixParticipantsResponseDto>
{
    public async Task<PixParticipantsResponseDto> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var activeParticipants = new List<PixActiveParticipantDto>();
        var adhesionParticipants = new List<PixAdhesionParticipantDto>();

        await foreach (var participant in repository.StreamActiveParticipantsAsync(cancellationToken))
        {
            activeParticipants.Add(activeMapper.Map(participant));
        }

        await foreach (var participant in repository.StreamAdhesionParticipantsAsync(cancellationToken))
        {
            adhesionParticipants.Add(adhesionMapper.Map(participant));
        }

        return new PixParticipantsResponseDto(activeParticipants, adhesionParticipants);
    }
}
