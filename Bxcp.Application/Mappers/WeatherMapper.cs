using Bxcp.Application.DTOs;
using Bxcp.Domain.Models;

namespace Bxcp.Application.Mappers;

/// <summary>
/// Maps between domain models and DTOs for weather data
/// </summary>
public static class WeatherMapper
{
    /// <summary>
    /// Maps a domain model to a DTO
    /// </summary>
    /// <param name="weatherRecord">The domain model to map</param>
    /// <returns>A DTO representing the weather analysis result</returns>
    public static WeatherAnalysisResult ToWeatherAnalysisResult(WeatherRecord weatherRecord)
    {
        return new WeatherAnalysisResult
        {
            DayWithSmallestTemperatureSpread = weatherRecord.Day,
            SmallestTemperatureSpread = weatherRecord.TemperatureSpread
        };
    }
}