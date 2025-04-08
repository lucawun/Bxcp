using Bxcp.Domain.Models;

namespace Bxcp.Domain.Ports;

/// <summary>
/// Interface for population density calculation service.
/// </summary>
public interface IPopulationDensityCalculator
{
    /// <summary>
    /// Finds the country with the highest population density.
    /// </summary>
    CountryRecord FindHighestPopulationDensity(IEnumerable<CountryRecord> countries);
}