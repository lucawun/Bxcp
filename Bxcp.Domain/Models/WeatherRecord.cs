namespace Bxcp.Domain.Models;

/// <summary>
/// Represents weather data for a specific day.
/// </summary>
public record WeatherRecord
{
    public int Day { get; init; }
    public double MaxTemperature { get; init; }
    public double MinTemperature { get; init; }

    /// <summary>
    /// Calculates the temperature spread between max and min temperatures.
    /// </summary>
    public double TemperatureSpread => MaxTemperature - MinTemperature;

    public double AverageTemperature { get; init; }
    public double AverageDewPoint { get; init; }
    public double Precipitation { get; init; }
    public int WindDirection { get; init; }
    public double AverageWindSpeed { get; init; }
    public int MaxWindSpeed { get; init; }
}