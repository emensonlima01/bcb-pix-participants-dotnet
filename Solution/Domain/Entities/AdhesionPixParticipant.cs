using System.Text.Json.Serialization;

namespace Domain.Entities;

public sealed class AdhesionPixParticipant : PixParticipant
{
    [JsonPropertyName("adhesionStatus")]
    public string AdhesionStatus { get; init; } = string.Empty;
}
