using Application.DTOs.Responses;
using Application.Interfaces;
using Application.Interfaces.Mappers;
using Application.Mappers;
using Application.UseCases;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services
            .AddMappers()
            .AddUseCases();

        return services;
    }

    private static IServiceCollection AddMappers(this IServiceCollection services)
    {
        services.AddSingleton<IMapper<ActivePixParticipant, PixActiveParticipantDto>, ActivePixParticipantMapper>();
        services.AddSingleton<IMapper<AdhesionPixParticipant, PixAdhesionParticipantDto>, AdhesionPixParticipantMapper>();

        return services;
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<ListPixParticipantsUseCase>();
        services.AddScoped<GetPixParticipantsUseCase>();
        services.AddScoped<StreamPixParticipantsUseCase>();

        return services;
    }
}
