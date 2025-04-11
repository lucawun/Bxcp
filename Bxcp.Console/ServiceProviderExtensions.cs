using Bxcp.Application.DTOs;
using Bxcp.Application.Ports.Driving;
using Bxcp.Application.UseCases;
using Bxcp.Domain.DomainServices;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Bxcp.Infrastructure.Adapters;
using Bxcp.Infrastructure.DataAccess;
using Microsoft.Extensions.DependencyInjection;

namespace Bxcp.Console;

public static class ServiceProviderExtensions
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, string weatherFilePath, string countriesFilePath)
    {
        // Register Domain Services
        services.AddDomainLayer();

        // Register Use Cases
        services.AddApplicationLayer();

        // Register Repositories
        services.AddInfrastructureLayer(weatherFilePath, countriesFilePath);

        return services;
    }

    public static IServiceCollection AddDomainLayer(this IServiceCollection services)
    {
        return services
            .AddSingleton<IClimateService, ClimateService>()
            .AddSingleton<ICountryStatisticsService, CountryStatisticsService>();
    }

    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        return services
            .AddSingleton<IClimateAnalysisUsecase, ClimateAnalysisUsecase>()
            .AddSingleton<ICountryAnalysisStatisticsUsecase, CountryAnalysisStratisticsUsecase>();
    }

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, string weatherFilePath, string countriesFilePath)
    {
        return services
            .AddSingleton(new CsvCountryFileReader(countriesFilePath))
            .AddSingleton(new CsvWeatherFileReader(weatherFilePath))
            .AddSingleton<IDataProviderRepository<Country>, CsvCountryRepository>()
            .AddSingleton<IDataProviderRepository<Weather>, CsvWeatherRepository>();
    }

    public static void RunWeatherAnalysis(this ServiceProvider serviceProvider)
    {
        try
        {
            IClimateAnalysisUsecase weatherAnalysisPort = serviceProvider.GetRequiredService<IClimateAnalysisUsecase>();
            ClimateAnalysisResult result = weatherAnalysisPort.AnalyzeClimate();

            System.Console.WriteLine("===== Weather Analysis =====");
            System.Console.WriteLine($"Day with smallest temperature range: Day {result.DayWithSmallestTemperatureSpread}");
            System.Console.WriteLine($"Smallest temperature range: {result.SmallestTemperatureSpread:F2}°C");
            System.Console.WriteLine();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error during weather analysis: {ex.Message}");
        }
    }

    public static void RunCountryAnalysis(this ServiceProvider serviceProvider)
    {
        try
        {
            ICountryAnalysisStatisticsUsecase countryAnalysisPort = serviceProvider.GetRequiredService<ICountryAnalysisStatisticsUsecase>();
            CountryAnalysisResult result = countryAnalysisPort.AnalyzeCountryStatistics();

            System.Console.WriteLine("===== Country Analysis =====");
            System.Console.WriteLine($"Country with highest population density: {result.CountryWithHighestDensity}");
            System.Console.WriteLine($"Highest population density: {result.HighestDensity:F2} inhabitants per km²");
            System.Console.WriteLine();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine($"Error during country analysis: {ex.Message}");
        }
    }
}