using System.Text.Json.Serialization;

namespace Application.DTOs.Responses;

public sealed record PixActiveParticipantDto(
    [property: JsonPropertyName("order")] int? Order,
    [property: JsonPropertyName("shortName")] string ShortName,
    [property: JsonPropertyName("ispb")] string Ispb,
    [property: JsonPropertyName("cnpj")] string Cnpj,
    [property: JsonPropertyName("institutionType")] string InstitutionType,
    [property: JsonPropertyName("bcbAuthorized")] string BcbAuthorized,
    [property: JsonPropertyName("spiParticipation")] string SpiParticipation,
    [property: JsonPropertyName("pixParticipation")] string PixParticipation,
    [property: JsonPropertyName("pixMode")] string PixMode,
    [property: JsonPropertyName("paymentInitiation")] string PaymentInitiation,
    [property: JsonPropertyName("cashoutFacilitator")] string CashoutFacilitator
);
