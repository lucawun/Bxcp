using Bxcp.Application.DTOs;

namespace Bxcp.Application.Ports.Primary;

/// <summary>
/// Primary port for data analysis operations
/// This interface defines the entry points to the application core
/// </summary>
public interface IDataAnalysisService
{
    /// <summary>
    /// Analyzes weather data to find the day with the smallest temperature spread
    /// </summary>
    /// <param name="filePath">Path to the weather data file</param>
    /// <returns>Analysis result containing the day with smallest temperature spread</returns>
    WeatherAnalysisResult AnalyzeWeatherData(string filePath);

    /// <summary>
    /// Analyzes country data to find the country with the highest population density
    /// </summary>
    /// <param name="filePath">Path to the country data file</param>
    /// <returns>Analysis result containing the country with highest population density</returns>
    CountryAnalysisResult AnalyzeCountryData(string filePath);
}