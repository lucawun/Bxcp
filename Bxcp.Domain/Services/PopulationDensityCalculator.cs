using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Domain.Services;

/// <summary>
/// Implements population density calculation logic.
/// </summary>
public class PopulationDensityCalculator : IPopulationDensityCalculator
{
    public CountryRecord FindHighestPopulationDensity(IEnumerable<CountryRecord> countries)
    {
        if (countries is null || !countries.Any())
            throw new ArgumentException("Country records cannot be null or empty.", nameof(countries));

        return countries
            .OrderByDescending(country => country.PopulationDensity)
            .First();
    }
}