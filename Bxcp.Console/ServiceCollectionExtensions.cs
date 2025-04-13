using Bxcp.Application.DTOs;
using Bxcp.Application.Ports.Driving;
using Bxcp.Application.UseCases;
using Bxcp.Domain.DomainServices;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Bxcp.Infrastructure.Adapters;
using Bxcp.Infrastructure.DataAccess.CsvHelper;
using Microsoft.Extensions.DependencyInjection;

namespace Bxcp.Console;

public static class ServiceCollectionExtensions
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

    public static IServiceCollection AddDomainLayer(this IServiceCollection services) => services
            .AddSingleton<IClimateService, ClimateService>()
            .AddSingleton<ICountryStatisticsService, CountryStatisticsService>();

    public static IServiceCollection AddApplicationLayer(this IServiceCollection services) => services
            .AddSingleton<IClimateAnalysisUseCase, ClimateAnalysisUseCase>()
            .AddSingleton<ICountryAnalysisStatisticsUseCase, CountryAnalysisStratisticsUseCase>();

    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, string weatherFilePath, string countriesFilePath) => services
            .AddSingleton(new CsvCountryFileReader(countriesFilePath))
            .AddSingleton(new CsvWeatherFileReader(weatherFilePath))
            .AddSingleton<IDataProviderRepository<Country>, CsvCountryRepository>()
            .AddSingleton<IDataProviderRepository<Weather>, CsvWeatherRepository>();

}