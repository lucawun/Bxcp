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
    public void ServiceProvider_ConfigureServices_RegistersAllDependencies()
    {
        // Act
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(_weatherFilePath, _countriesFilePath)
            .BuildServiceProvider();

        // Assert
        IClimateAnalysisUsecase? climateAnalysisUseCase = serviceProvider.GetService<IClimateAnalysisUsecase>();
        ICountryAnalysisStatisticsUsecase? countryAnalysisUseCase = serviceProvider.GetService<ICountryAnalysisStatisticsUsecase>();

        Assert.NotNull(climateAnalysisUseCase);
        Assert.NotNull(countryAnalysisUseCase);
    }

    [Fact]
    public void RunWeatherAnalysis_WithValidData_ReturnsCorrectResult()
    {
        // Arrange
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(_weatherFilePath, _countriesFilePath)
            .BuildServiceProvider();

        IClimateAnalysisUsecase climateAnalysisUseCase = serviceProvider.GetRequiredService<IClimateAnalysisUsecase>();

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
    public void RunCountryAnalysis_WithValidData_ReturnsCorrectResult()
    {
        // Arrange
        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(_weatherFilePath, _countriesFilePath)
            .BuildServiceProvider();

        ICountryAnalysisStatisticsUsecase countryAnalysisUseCase = serviceProvider.GetRequiredService<ICountryAnalysisStatisticsUsecase>();

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
    public void ServiceProvider_WithInvalidFilePaths_ThrowsExceptionWhenUsed()
    {
        // Arrange
        string invalidWeatherPath = "nonexistent_weather.csv";
        string invalidCountriesPath = "nonexistent_countries.csv";

        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(invalidWeatherPath, invalidCountriesPath)
            .BuildServiceProvider();

        IClimateAnalysisUsecase climateAnalysisUseCase = serviceProvider.GetRequiredService<IClimateAnalysisUsecase>();
        ICountryAnalysisStatisticsUsecase countryAnalysisUseCase = serviceProvider.GetRequiredService<ICountryAnalysisStatisticsUsecase>();

        // Act & Assert - Check for inner exception
        AnalysisFailedException exception = Assert.Throws<AnalysisFailedException>(() => climateAnalysisUseCase.AnalyzeClimate());
        Assert.IsType<FileNotFoundException>(exception.InnerException);

        AnalysisFailedException exception2 = Assert.Throws<AnalysisFailedException>(() => countryAnalysisUseCase.AnalyzeCountryStatistics());
        Assert.IsType<FileNotFoundException>(exception2.InnerException);
    }

    [Fact]
    public void DomainLayer_RegistersExpectedServices()
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