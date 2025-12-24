namespace Application.DTOs;

public sealed record PixParticipantsResponse(
    IReadOnlyList<PixParticipantItem> Participants
);

public sealed record PixParticipantItem(
    string Ispb,
    string Name
);
