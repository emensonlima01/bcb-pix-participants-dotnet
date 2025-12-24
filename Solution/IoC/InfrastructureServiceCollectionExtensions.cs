using Domain.Interfaces;
using Infrastructure.Options;
using Infrastructure.PixParticipants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class InfrastructureServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddPixParticipants(configuration)
            .AddRepositories()
            .AddDomainServices();

        return services;
    }

    private static IServiceCollection AddPixParticipants(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PixParticipantsOptions>(configuration.GetSection("PixParticipants"));
        services.AddHttpClient<IPixParticipantsSource, BcbPixParticipantsSource>();

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services;
    }

    private static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        return services;
    }
}
