using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Infrastructure.Options;
using Infrastructure.Parsers;
using Infrastructure.PixParticipants;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDataSources(configuration)
            .AddParsers()
            .AddRepositories()
            .AddDomainServices();

        return services;
    }

    private static IServiceCollection AddDataSources(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PixParticipantsOptions>(configuration.GetSection("PixParticipants"));

        services.AddHttpClient<IDataSourceReader, BcbCsvDataSourceReader>();
        services.AddHttpClient<IPixParticipantsSource, BcbPixParticipantsSource>();

        return services;
    }

    private static IServiceCollection AddParsers(this IServiceCollection services)
    {
        services.AddSingleton<ICsvLineParser<ActivePixParticipant>, ActivePixParticipantParser>();
        services.AddSingleton<ICsvLineParser<AdhesionPixParticipant>, AdhesionPixParticipantParser>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPixParticipantsRepository, PixParticipantsRepository>();

        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}
