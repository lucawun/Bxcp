using Bxcp.Application.DTOs;
using Bxcp.Application.Exceptions;
using Bxcp.Application.Ports.Driving;
using Bxcp.Console;
using Bxcp.Domain.Ports;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Bxcp.Integration.Tests.EndToEnd;

public class IntegrationTests
{
    private readonly string _weatherFilePath;
    private readonly string _countriesFilePath;

    public IntegrationTests()
    {
        _weatherFilePath = Path.Combine("TestData", "weather.csv");
        _countriesFilePath = Path.Combine("TestData", "countries.csv");
    }

    [Fact]
    public void ServiceProviderConfigureServicesRegistersAllDependencies()
    {
        // Act
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(_weatherFilePath, _countriesFilePath)
            .BuildServiceProvider();

        // Assert
        IClimateAnalysisUseCase? climateAnalysisUseCase = serviceProvider.GetService<IClimateAnalysisUseCase>();
        ICountryAnalysisStatisticsUseCase? countryAnalysisUseCase = serviceProvider.GetService<ICountryAnalysisStatisticsUseCase>();

        Assert.NotNull(climateAnalysisUseCase);
        Assert.NotNull(countryAnalysisUseCase);
    }

    [Fact]
    public void RunWeatherAnalysisWithValidDataReturnsCorrectResult()
    {
        // Arrange
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(_weatherFilePath, _countriesFilePath)
            .BuildServiceProvider();

        IClimateAnalysisUseCase climateAnalysisUseCase = serviceProvider.GetRequiredService<IClimateAnalysisUseCase>();

        // Act
        ClimateAnalysisResult result = climateAnalysisUseCase.AnalyzeClimate();

        // Assert
        Assert.IsType<ClimateAnalysisResult>(result);
        Assert.True(result.DayWithSmallestTemperatureSpread > 0);
        Assert.True(result.SmallestTemperatureSpread >= 0);

        // Based on the sample data, we expect day 14 to have the smallest temperature spread
        Assert.Equal(14, result.DayWithSmallestTemperatureSpread);
        Assert.Equal(2.0, result.SmallestTemperatureSpread);
    }

    [Fact]
    public void RunCountryAnalysisWithValidDataReturnsCorrectResult()
    {
        // Arrange
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(_weatherFilePath, _countriesFilePath)
            .BuildServiceProvider();

        ICountryAnalysisStatisticsUseCase countryAnalysisUseCase = serviceProvider.GetRequiredService<ICountryAnalysisStatisticsUseCase>();

        // Act
        CountryAnalysisResult result = countryAnalysisUseCase.AnalyzeCountryStatistics();

        // Assert
        Assert.IsType<CountryAnalysisResult>(result);
        Assert.False(string.IsNullOrEmpty(result.CountryWithHighestDensity));
        Assert.True(result.HighestDensity > 0);

        // Malta should be the country with highest density in the sample data
        Assert.Equal("Malta", result.CountryWithHighestDensity);
        Assert.True(result.HighestDensity > 1000); // Malta's density is more than 1000 people per km²
    }

    [Fact]
    public void ServiceProviderWithInvalidFilePathsThrowsExceptionWhenUsed()
    {
        // Arrange
        const string invalidWeatherPath = "nonexistent_weather.csv";
        const string invalidCountriesPath = "nonexistent_countries.csv";

        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(invalidWeatherPath, invalidCountriesPath)
            .BuildServiceProvider();

        IClimateAnalysisUseCase climateAnalysisUseCase = serviceProvider.GetRequiredService<IClimateAnalysisUseCase>();
        ICountryAnalysisStatisticsUseCase countryAnalysisUseCase = serviceProvider.GetRequiredService<ICountryAnalysisStatisticsUseCase>();

        // Act & Assert - Check for inner exception
        AnalysisFailedException exception = Assert.Throws<AnalysisFailedException>(() => climateAnalysisUseCase.AnalyzeClimate());
        Assert.IsType<FileNotFoundException>(exception.InnerException);

        AnalysisFailedException exception2 = Assert.Throws<AnalysisFailedException>(() => countryAnalysisUseCase.AnalyzeCountryStatistics());
        Assert.IsType<FileNotFoundException>(exception2.InnerException);
    }

    [Fact]
    public void DomainLayerRegistersExpectedServices()
    {
        // Act
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddDomainLayer()
            .BuildServiceProvider();

        // Assert
        IClimateService? climateService = serviceProvider.GetService<IClimateService>();
        ICountryStatisticsService? countryService = serviceProvider.GetService<ICountryStatisticsService>();

        Assert.NotNull(climateService);
        Assert.NotNull(countryService);
    }
}