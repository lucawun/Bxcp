using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Domain.DomainServices;

public class CountryStatisticsService : ICountryStatisticsService
{
    /// <summary>
    /// Finds the country with the highest population density
    /// </summary>
    /// <param name="countries">A collection of country objects to analyze</param>
    /// <returns>The country with the highest population density</returns>
    /// <exception cref="DomainException">Thrown when the countries collection is null or empty</exception>
    public Country FindHighestPopulationDensity(IEnumerable<Country> countries)
    {
        if (countries?.Any() != true)
            throw new DomainException("Country records cannot be null or empty.");

        return countries
            .OrderByDescending(country => country.PopulationDensity)
            .First();
    }
}