using Application.UseCases;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IoC;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddUseCases();

        return services;
    }

    private static IServiceCollection AddUseCases(this IServiceCollection services)
    {
        services.AddHttpClient();

        services.AddScoped(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            var csvUrl = configuration["PixParticipants:CsvUrl"];
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient();
            return new ListPixParticipantsUseCase(csvUrl, httpClient);
        });

        return services;
    }
}
