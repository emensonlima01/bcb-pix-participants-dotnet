using Application.DTOs.Responses;
using System.Text.Json;

namespace WebApi.Serializers;

public static class PixParticipantsJsonSerializer
{
    public static async Task SerializeAsync(
        Stream stream,
        IAsyncEnumerable<PixActiveParticipantDto> activeParticipants,
        IAsyncEnumerable<PixAdhesionParticipantDto> adhesionParticipants,
        CancellationToken cancellationToken = default)
    {
        await using var writer = new Utf8JsonWriter(stream);
        
        writer.WriteStartObject();
        
        writer.WritePropertyName("activeParticipants");
        writer.WriteStartArray();
        await foreach (var participant in activeParticipants.WithCancellation(cancellationToken))
        {
            SerializeActiveParticipant(writer, participant);
            await writer.FlushAsync(cancellationToken);
        }
        writer.WriteEndArray();

        writer.WritePropertyName("adhesionParticipants");
        writer.WriteStartArray();
        await foreach (var participant in adhesionParticipants.WithCancellation(cancellationToken))
        {
            SerializeAdhesionParticipant(writer, participant);
            await writer.FlushAsync(cancellationToken);
        }
        writer.WriteEndArray();

        writer.WriteEndObject();
        await writer.FlushAsync(cancellationToken);
    }

    private static void SerializeActiveParticipant(Utf8JsonWriter writer, PixActiveParticipantDto participant)
    {
        writer.WriteStartObject();
        WriteNullableNumber(writer, "order", participant.Order);
        writer.WriteString("shortName", participant.ShortName);
        writer.WriteString("ispb", participant.Ispb);
        writer.WriteString("cnpj", participant.Cnpj);
        writer.WriteString("institutionType", participant.InstitutionType);
        writer.WriteString("bcbAuthorized", participant.BcbAuthorized);
        writer.WriteString("spiParticipation", participant.SpiParticipation);
        writer.WriteString("pixParticipation", participant.PixParticipation);
        writer.WriteString("pixMode", participant.PixMode);
        writer.WriteString("paymentInitiation", participant.PaymentInitiation);
        writer.WriteString("cashoutFacilitator", participant.CashoutFacilitator);
        writer.WriteEndObject();
    }

    private static void SerializeAdhesionParticipant(Utf8JsonWriter writer, PixAdhesionParticipantDto participant)
    {
        writer.WriteStartObject();
        WriteNullableNumber(writer, "order", participant.Order);
        writer.WriteString("shortName", participant.ShortName);
        writer.WriteString("ispb", participant.Ispb);
        writer.WriteString("cnpj", participant.Cnpj);
        writer.WriteString("institutionType", participant.InstitutionType);
        writer.WriteString("bcbAuthorized", participant.BcbAuthorized);
        writer.WriteString("spiParticipation", participant.SpiParticipation);
        writer.WriteString("pixParticipation", participant.PixParticipation);
        writer.WriteString("pixMode", participant.PixMode);
        writer.WriteString("adhesionStatus", participant.AdhesionStatus);
        writer.WriteEndObject();
    }

    private static void WriteNullableNumber(Utf8JsonWriter writer, string propertyName, int? value)
    {
        if (value.HasValue)
        {
            writer.WriteNumber(propertyName, value.Value);
        }
        else
        {
            writer.WriteNull(propertyName);
        }
    }
}
