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
        services.Configure<PixParticipantsOptions>(configuration.GetSection("PixParticipants"));

        services.AddHttpClient<IDataSourceReader, BcbCsvDataSourceReader>();
        services.AddHttpClient<IPixParticipantsSource, BcbPixParticipantsSource>();

        services.AddSingleton<ICsvLineParser<ActivePixParticipant>, ActivePixParticipantParser>();
        services.AddSingleton<ICsvLineParser<AdhesionPixParticipant>, AdhesionPixParticipantParser>();

        services.AddScoped<IPixParticipantsRepository, PixParticipantsRepository>();

        return services;
    }
}
