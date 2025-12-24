using System.Text.Json.Serialization;

namespace Domain.Entities;

public sealed class ActivePixParticipant : PixParticipant
{
    [JsonPropertyName("paymentInitiation")]
    public string PaymentInitiation { get; init; } = string.Empty;

    [JsonPropertyName("cashoutFacilitator")]
    public string CashoutFacilitator { get; init; } = string.Empty;
}
