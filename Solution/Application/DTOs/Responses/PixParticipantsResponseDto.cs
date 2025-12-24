using System.Text.Json.Serialization;

namespace Application.DTOs.Responses;

public sealed record PixParticipantsResponseDto(
    [property: JsonPropertyName("activeParticipants")] IReadOnlyList<PixActiveParticipantDto> ActiveParticipants,
    [property: JsonPropertyName("adhesionParticipants")] IReadOnlyList<PixAdhesionParticipantDto> AdhesionParticipants
);
