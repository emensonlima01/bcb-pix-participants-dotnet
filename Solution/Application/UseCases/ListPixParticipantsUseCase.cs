using Application.DTOs;

namespace Application.UseCases;

public sealed class ListPixParticipantsUseCase
{
    public Task<PixParticipantsResponse> Handle(CancellationToken cancellationToken = default)
    {
        // TODO: Replace with the real Pix participants source (BCB directory or database).
        var participants = Array.Empty<PixParticipantItem>();
        return Task.FromResult(new PixParticipantsResponse(participants));
    }
}
