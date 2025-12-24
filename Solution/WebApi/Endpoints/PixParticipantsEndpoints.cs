using Application.DTOs;
using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace WebApi.Endpoints;

public static class PixParticipantsEndpoints
{
    public static void MapPixParticipantsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/pix")
            .WithTags("Pix");

        group.MapGet("/participants", StreamParticipants)
            .WithName("PixParticipantsList");
    }

    private static Task<IResult> StreamParticipants(
        [FromServices] ListPixParticipantsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var stream = useCase.Handle(cancellationToken);
        return Task.FromResult(Results.Stream(async responseStream =>
        {
            await using var writer = new Utf8JsonWriter(responseStream);
            writer.WriteStartObject();
            writer.WritePropertyName(ActiveSectionName);
            writer.WriteStartArray();

            var adhesionStarted = false;

            await foreach (var item in stream.WithCancellation(cancellationToken))
            {
                if (item.Kind == PixParticipantKind.Active)
                {
                    if (item.ActiveParticipant is null)
                    {
                        continue;
                    }

                    WriteActiveParticipant(writer, item.ActiveParticipant);
                }
                else
                {
                    if (!adhesionStarted)
                    {
                        writer.WriteEndArray();
                        writer.WritePropertyName(AdhesionSectionName);
                        writer.WriteStartArray();
                        adhesionStarted = true;
                    }

                    if (item.AdhesionParticipant is null)
                    {
                        continue;
                    }

                    WriteAdhesionParticipant(writer, item.AdhesionParticipant);
                }

                await writer.FlushAsync(cancellationToken);
            }

            if (!adhesionStarted)
            {
                writer.WriteEndArray();
                writer.WritePropertyName(AdhesionSectionName);
                writer.WriteStartArray();
                writer.WriteEndArray();
            }
            else
            {
                writer.WriteEndArray();
            }

            writer.WriteEndObject();
            await writer.FlushAsync(cancellationToken);
        }, "application/json; charset=utf-8"));
    }

    private const string ActiveSectionName = "activeParticipants";
    private const string AdhesionSectionName = "adhesionParticipants";

    private static void WriteActiveParticipant(Utf8JsonWriter writer, PixActiveParticipant participant)
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

    private static void WriteAdhesionParticipant(Utf8JsonWriter writer, PixAdhesionParticipant participant)
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
