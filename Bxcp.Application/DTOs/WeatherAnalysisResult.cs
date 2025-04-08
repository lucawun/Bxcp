namespace Bxcp.Application.DTOs;

/// <summary>
/// Data Transfer Object containing the results of a weather data analysis
/// </summary>
public record WeatherAnalysisResult
{
    /// <summary>
    /// The day number with the smallest temperature spread
    /// </summary>
    public int DayWithSmallestTemperatureSpread { get; init; }

    /// <summary>
    /// The value of the smallest temperature spread
    /// </summary>
    public double SmallestTemperatureSpread { get; init; }
}