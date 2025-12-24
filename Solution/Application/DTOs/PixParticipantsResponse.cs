using System.Text.Json.Serialization;

namespace Application.DTOs;

public sealed record PixParticipantsResponse(
    [property: JsonPropertyName("Lista de participantes ativos do Pix")] IReadOnlyList<PixActiveParticipant> ActiveParticipants,
    [property: JsonPropertyName("Lista de instituicoes em processo de adesao ao Pix")] IReadOnlyList<PixAdhesionParticipant> AdhesionParticipants
);

public sealed record PixActiveParticipant(
    [property: JsonPropertyName("Numero")] int? Order,
    [property: JsonPropertyName("Nome Reduzido")] string Name,
    [property: JsonPropertyName("ISPB")] string Ispb,
    [property: JsonPropertyName("CNPJ")] string Cnpj,
    [property: JsonPropertyName("Tipo de Instituicao")] string InstitutionType,
    [property: JsonPropertyName("Autorizada pelo BCB")] string AuthorizedByBcb,
    [property: JsonPropertyName("Tipo de Participacao no SPI")] string SpiParticipationType,
    [property: JsonPropertyName("Tipo de Participacao no Pix")] string PixParticipationType,
    [property: JsonPropertyName("Modalidade de Participacao no Pix")] string PixParticipationMode,
    [property: JsonPropertyName("Iniciacao de Transacao de Pagamento")] string PaymentInitiation,
    [property: JsonPropertyName("Facilitador de servico de Saque e Troco (FSS)")] string CashoutFacilitator
);

public sealed record PixAdhesionParticipant(
    [property: JsonPropertyName("Numero")] int? Order,
    [property: JsonPropertyName("Nome Reduzido")] string Name,
    [property: JsonPropertyName("ISPB")] string Ispb,
    [property: JsonPropertyName("CNPJ")] string Cnpj,
    [property: JsonPropertyName("Tipo de Instituicao")] string InstitutionType,
    [property: JsonPropertyName("Autorizada pelo BCB")] string AuthorizedByBcb,
    [property: JsonPropertyName("Tipo de Participacao no SPI")] string SpiParticipationType,
    [property: JsonPropertyName("Tipo de Participacao no Pix")] string PixParticipationType,
    [property: JsonPropertyName("Modalidade de Participacao no Pix")] string PixParticipationMode,
    [property: JsonPropertyName("Status da adesao")] string AdhesionStatus
);
