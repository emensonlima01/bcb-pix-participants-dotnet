using Domain.Entities;

namespace Domain.Interfaces.Repositories;

public interface IPixParticipantsRepository
{
    IAsyncEnumerable<ActivePixParticipant> StreamActiveParticipantsAsync(CancellationToken cancellationToken = default);
    IAsyncEnumerable<AdhesionPixParticipant> StreamAdhesionParticipantsAsync(CancellationToken cancellationToken = default);
}
