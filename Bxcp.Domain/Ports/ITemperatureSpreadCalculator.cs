using Bxcp.Domain.Models;

namespace Bxcp.Domain.Ports;

/// <summary>
/// Interface for temperature spread calculation service.
/// </summary>
public interface ITemperatureSpreadCalculator
{
    /// <summary>
    /// Finds the record with the smallest temperature spread.
    /// </summary>
    WeatherRecord FindSmallestTemperatureSpread(IEnumerable<WeatherRecord> records);
}
