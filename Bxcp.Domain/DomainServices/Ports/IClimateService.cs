using Bxcp.Domain.Models;

namespace Bxcp.Domain.DomainServices.Ports;

public interface IClimateService
{
    /// <summary>
    /// Finds the record with the smallest temperature spread.
    /// </summary>
    Weather FindSmallestTemperatureSpread(IEnumerable<Weather> records);
}
