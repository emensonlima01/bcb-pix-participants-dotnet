using Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Endpoints;

public static class PixParticipantsEndpoints
{
    public static void MapPixParticipantsEndpoints(this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/pix")
            .WithTags("Pix");

        group.MapGet("/participants", ListParticipants)
            .WithName("PixParticipantsList");
    }

    private static async Task<IResult> ListParticipants(
        [FromServices] ListPixParticipantsUseCase useCase,
        CancellationToken cancellationToken)
    {
        var response = await useCase.Handle(cancellationToken);
        return Results.Ok(response);
    }
}
