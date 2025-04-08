using Bxcp.Application.DTOs;
using Bxcp.Application.Mappers;
using Bxcp.Application.Ports.Primary;
using Bxcp.Application.Ports.Secondary;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Bxcp.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Bxcp.Application.Services;

/// <summary>
/// Implementation of the primary port IDataAnalysisService
/// This service orchestrates the core application logic
/// </summary>
public class DataAnalysisService : IDataAnalysisService
{
    private readonly IFileReader<WeatherRecord> _weatherFileReader;
    private readonly IFileReader<CountryRecord> _countryFileReader;
    private readonly IPopulationDensityCalculator _densityCalculator;
    private readonly ILogger<DataAnalysisService> _logger;

    public DataAnalysisService(
        IFileReader<WeatherRecord> weatherFileReader,
        IFileReader<CountryRecord> countryFileReader,
        IPopulationDensityCalculator densityCalculator,
        ITemperatureSpreadCalculator temperatureSpreadCalculator,
            ILogger<DataAnalysisService> logger)
    {
        _weatherFileReader = weatherFileReader ?? throw new ArgumentNullException(nameof(weatherFileReader));
        _countryFileReader = countryFileReader ?? throw new ArgumentNullException(nameof(countryFileReader));
        _densityCalculator = densityCalculator ?? throw new ArgumentNullException(nameof(densityCalculator));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Analyzes weather data to find the day with the smallest temperature spread
    /// </summary>
    public WeatherAnalysisResult AnalyzeWeatherData(string filePath)
    {
        IEnumerable<WeatherRecord> weatherRecords = _weatherFileReader.ReadAllRecords(filePath);
        if (!weatherRecords.Any())
        {
            throw new EmptyDataException("No weather data found.");
        }

        WeatherRecord dayWithSmallestSpread = weatherRecords
            .OrderBy(record => record.TemperatureSpread)
            .First();

        return new WeatherAnalysisResult
        {
            DayWithSmallestTemperatureSpread = dayWithSmallestSpread.Day,
            SmallestTemperatureSpread = dayWithSmallestSpread.TemperatureSpread
        };
    }

    /// <summary>
    /// Analyzes country data to find the country with the highest population density
    /// </summary>
    public CountryAnalysisResult AnalyzeCountryData(string filePath)
    {
        _logger.LogInformation("Analyzing country data from file: {FilePath}", filePath);

        try
        {
            // Read all country records from the file
            var records = _countryFileReader.ReadAllRecords(filePath);

            if (records == null || !records.Any())
            {
                _logger.LogWarning("No country records found in file: {FilePath}", filePath);
                return new CountryAnalysisResult
                {
                    CountryWithHighestDensity = string.Empty,
                    HighestDensity = 0
                };
            }

            // Delegate the business logic of finding highest density to the domain service
            CountryRecord countryWithHighestDensity = _densityCalculator.FindHighestPopulationDensity(records);

            _logger.LogInformation("Found country with highest population density: {Country} with density {Density}",
                countryWithHighestDensity.Name, countryWithHighestDensity.PopulationDensity);

          
            return CountryMapper.ToCountryAnalysisResult(countryWithHighestDensity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing country data from file: {FilePath}", filePath);
            throw new ApplicationException($"Failed to analyze country data: {ex.Message}", ex);
        }
    }
}
