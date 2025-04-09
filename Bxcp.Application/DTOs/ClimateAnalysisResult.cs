namespace Bxcp.Application.DTOs;

/// <summary>
/// Data Transfer Object containing the results of a climate analysis
/// </summary>
public record ClimateAnalysisResult
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