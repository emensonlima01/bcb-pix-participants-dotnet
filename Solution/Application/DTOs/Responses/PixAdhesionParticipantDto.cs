using System.Text.Json.Serialization;

namespace Application.DTOs.Responses;

public sealed record PixAdhesionParticipantDto(
    [property: JsonPropertyName("order")] int? Order,
    [property: JsonPropertyName("shortName")] string ShortName,
    [property: JsonPropertyName("ispb")] string Ispb,
    [property: JsonPropertyName("cnpj")] string Cnpj,
    [property: JsonPropertyName("institutionType")] string InstitutionType,
    [property: JsonPropertyName("bcbAuthorized")] string BcbAuthorized,
    [property: JsonPropertyName("spiParticipation")] string SpiParticipation,
    [property: JsonPropertyName("pixParticipation")] string PixParticipation,
    [property: JsonPropertyName("pixMode")] string PixMode,
    [property: JsonPropertyName("adhesionStatus")] string AdhesionStatus
);
