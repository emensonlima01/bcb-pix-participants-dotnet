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

    private const string ActiveSectionName = "Lista de participantes ativos do Pix";
    private const string AdhesionSectionName = "Lista de instituicoes em processo de adesao ao Pix";

    private static void WriteActiveParticipant(Utf8JsonWriter writer, PixActiveParticipant participant)
    {
        writer.WriteStartObject();
        WriteNullableNumber(writer, "Numero", participant.Order);
        writer.WriteString("Nome Reduzido", participant.Name);
        writer.WriteString("ISPB", participant.Ispb);
        writer.WriteString("CNPJ", participant.Cnpj);
        writer.WriteString("Tipo de Instituicao", participant.InstitutionType);
        writer.WriteString("Autorizada pelo BCB", participant.AuthorizedByBcb);
        writer.WriteString("Tipo de Participacao no SPI", participant.SpiParticipationType);
        writer.WriteString("Tipo de Participacao no Pix", participant.PixParticipationType);
        writer.WriteString("Modalidade de Participacao no Pix", participant.PixParticipationMode);
        writer.WriteString("Iniciacao de Transacao de Pagamento", participant.PaymentInitiation);
        writer.WriteString("Facilitador de servico de Saque e Troco (FSS)", participant.CashoutFacilitator);
        writer.WriteEndObject();
    }

    private static void WriteAdhesionParticipant(Utf8JsonWriter writer, PixAdhesionParticipant participant)
    {
        writer.WriteStartObject();
        WriteNullableNumber(writer, "Numero", participant.Order);
        writer.WriteString("Nome Reduzido", participant.Name);
        writer.WriteString("ISPB", participant.Ispb);
        writer.WriteString("CNPJ", participant.Cnpj);
        writer.WriteString("Tipo de Instituicao", participant.InstitutionType);
        writer.WriteString("Autorizada pelo BCB", participant.AuthorizedByBcb);
        writer.WriteString("Tipo de Participacao no SPI", participant.SpiParticipationType);
        writer.WriteString("Tipo de Participacao no Pix", participant.PixParticipationType);
        writer.WriteString("Modalidade de Participacao no Pix", participant.PixParticipationMode);
        writer.WriteString("Status da adesao", participant.AdhesionStatus);
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
