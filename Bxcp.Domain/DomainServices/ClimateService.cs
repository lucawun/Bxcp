using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Domain.DomainServices;

public class ClimateService : IClimateService
{
    public Weather FindSmallestTemperatureSpread(IEnumerable<Weather> records)
    {
        if (records is null || !records.Any())
            throw new DomainException("Weather records cannot be null or empty.");

        return records
            .OrderBy(record => record.TemperatureSpread)
            .First();
    }
}
