using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Domain.Services;

/// <summary>
/// Implements temperature spread calculation logic.
/// </summary>
public class TemperatureSpreadCalculator : ITemperatureSpreadCalculator
{
    public WeatherRecord FindSmallestTemperatureSpread(IEnumerable<WeatherRecord> records)
    {
        if (records is null || !records.Any())
            throw new ArgumentException("Weather records cannot be null or empty.", nameof(records));

        return records
            .OrderBy(record => record.TemperatureSpread)
            .First();
    }
}
