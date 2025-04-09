using Bxcp.Application.DTOs;
using Bxcp.Domain.Models;

namespace Bxcp.Application.Mappers;

/// <summary>
/// Maps between domain models and DTOs for weather use case
/// </summary>
public static class ClimateAnalysisMapper
{
    /// <summary>
    /// Maps a domain model to a DTO
    /// </summary>
    /// <param name="weather">The domain model to map</param>
    /// <returns>A DTO representing the weather analysis result</returns>
    public static ClimateAnalysisResult ToClimateAnalysisResult(Weather weather)
    {
        return new ClimateAnalysisResult
        {
            DayWithSmallestTemperatureSpread = weather.Day,
            SmallestTemperatureSpread = weather.TemperatureSpread
        };
    }
}