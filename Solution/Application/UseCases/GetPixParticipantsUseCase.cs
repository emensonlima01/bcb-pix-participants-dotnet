using Application.DTOs;
using Domain.Entities;
using Domain.Interfaces.Repositories;

namespace Application.UseCases;

public sealed class GetPixParticipantsUseCase(IPixParticipantsRepository repository)
{
    public async Task<PixParticipantsResponse> ExecuteAsync(CancellationToken cancellationToken = default)
    {
        var activeParticipants = new List<ActivePixParticipant>();
        var adhesionParticipants = new List<AdhesionPixParticipant>();

        await foreach (var participant in repository.StreamActiveParticipantsAsync(cancellationToken))
        {
            activeParticipants.Add(participant);
        }

        await foreach (var participant in repository.StreamAdhesionParticipantsAsync(cancellationToken))
        {
            adhesionParticipants.Add(participant);
        }

        return new PixParticipantsResponse(activeParticipants, adhesionParticipants);
    }
}
