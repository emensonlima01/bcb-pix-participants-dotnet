using Application.UseCases;

namespace WebApi.Endpoints;

public static class PixParticipantsEndpoints
{
    public static void MapPixParticipantsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/pix")
            .WithTags("Pix");

        group.MapGet("/participants", GetParticipants)
            .WithName("GetPixParticipants")
            .WithDescription("Retorna a lista completa de participantes do Pix (ativos e em adesão)");
    }

    private static async Task<IResult> GetParticipants(
        GetPixParticipantsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var response = await useCase.ExecuteAsync(cancellationToken);
        return Results.Ok(response);
    }
}
