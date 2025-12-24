using Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using WebApi.Serializers;

namespace WebApi.Endpoints;

public static class PixParticipantsEndpoints
{
    public static void MapPixParticipantsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/pix")
            .WithTags("Pix");

        group.MapGet("/participants", StreamParticipants)
            .WithName("PixParticipantsList")
            .WithDescription("Retorna a lista de participantes do Pix (ativos e em adesão)");
    }

    private static Task<IResult> StreamParticipants(
        [FromServices] StreamPixParticipantsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var activeStream = useCase.StreamActiveAsync(cancellationToken);
        var adhesionStream = useCase.StreamAdhesionAsync(cancellationToken);

        return Task.FromResult(Results.Stream(async responseStream =>
        {
            await PixParticipantsJsonSerializer.SerializeAsync(
                responseStream,
                activeStream,
                adhesionStream,
                cancellationToken);
        }, "application/json; charset=utf-8"));
    }
}
