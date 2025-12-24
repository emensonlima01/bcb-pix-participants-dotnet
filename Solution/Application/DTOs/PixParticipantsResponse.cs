using System.Text.Json.Serialization;
using Domain.Entities;

namespace Application.DTOs;

public sealed record PixParticipantsResponse(
    [property: JsonPropertyName("activeParticipants")] IReadOnlyList<ActivePixParticipant> ActiveParticipants,
    [property: JsonPropertyName("adhesionParticipants")] IReadOnlyList<AdhesionPixParticipant> AdhesionParticipants
);
