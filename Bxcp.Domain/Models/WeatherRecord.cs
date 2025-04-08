using Bxcp.Domain.Exceptions;

namespace Bxcp.Domain.Models;

/// <summary>
/// Represents a single weather record (day, max/min temperature).
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

    /// <summary>
    /// Creates a new WeatherRecord, validating initial values.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown if day is zero or negative, or if MaxTemperature is less than MinTemperature.
    /// </exception>
    public WeatherRecord(int day, double maxTemperature, double minTemperature)
    {
        if (day <= 0)
            throw new DomainException("The day number must be a positive integer.");

        if (maxTemperature < minTemperature)
            throw new DomainException("Max temperature cannot be less than min temperature.");

        Day = day;
        MaxTemperature = maxTemperature;
        MinTemperature = minTemperature;
    }
}