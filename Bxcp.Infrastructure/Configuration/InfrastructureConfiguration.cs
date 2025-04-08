using Bxcp.Application.Ports.Secondary;
using Bxcp.Infrastructure.Adapters.FileSystem;
using Bxcp.Infrastructure.DTOs;
using Microsoft.Extensions.DependencyInjection;

namespace Bxcp.Infrastructure.Configuration;

/// <summary>
/// Configuration class for infrastructure services
/// </summary>
public static class InfrastructureConfiguration
{
    /// <summary>
    /// Adds infrastructure services to the DI container
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The updated service collection</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        // Register file readers
        services.AddScoped<IFileReader<WeatherCsvRecord>, CsvWeatherFileReader>();
        services.AddScoped<IFileReader<CountryCsvRecord>, CsvCountryFileReader>();

        return services;
    }
}