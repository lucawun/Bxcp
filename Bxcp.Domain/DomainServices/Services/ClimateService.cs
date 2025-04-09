using Bxcp.Domain.DomainServices.Ports;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;

namespace Bxcp.Domain.DomainServices.Services;

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
