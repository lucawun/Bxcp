using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Domain.DomainServices;

public class ClimateService : IClimateService
{
    /// <summary>
    /// Finds the weather record with the smallest temperature spread.
    /// Temperature spread is typically the difference between maximum and minimum temperature.
    /// </summary>
    /// <param name="records">A collection of weather records to analyze</param>
    /// <returns>The weather record with the smallest temperature spread</returns>
    /// <exception cref="DomainException">Thrown when the weather records collection is null or empty</exception>
    public Weather FindSmallestTemperatureSpread(IEnumerable<Weather> records)
    {
        if (records?.Any() != true)
            throw new DomainException("Weather records cannot be null or empty.");

        return records
            .OrderBy(record => record.TemperatureSpread)
            .First();
    }
}
