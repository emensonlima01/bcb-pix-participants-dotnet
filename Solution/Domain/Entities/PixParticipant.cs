using System.Text.Json.Serialization;

namespace Domain.Entities;

public abstract class PixParticipant
{
    [JsonPropertyName("order")]
    public int? Order { get; init; }

    [JsonPropertyName("shortName")]
    public string ShortName { get; init; } = string.Empty;

    [JsonPropertyName("ispb")]
    public string Ispb { get; init; } = string.Empty;

    [JsonPropertyName("cnpj")]
    public string Cnpj { get; init; } = string.Empty;

    [JsonPropertyName("institutionType")]
    public string InstitutionType { get; init; } = string.Empty;

    [JsonPropertyName("bcbAuthorized")]
    public string BcbAuthorized { get; init; } = string.Empty;

    [JsonPropertyName("spiParticipation")]
    public string SpiParticipation { get; init; } = string.Empty;

    [JsonPropertyName("pixParticipation")]
    public string PixParticipation { get; init; } = string.Empty;

    [JsonPropertyName("pixMode")]
    public string PixMode { get; init; } = string.Empty;
}
