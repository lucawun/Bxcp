using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Domain.DomainServices;

public class CountryStatisticsService : ICountryStatisticsService
{
    public Country FindHighestPopulationDensity(IEnumerable<Country> countries)
    {
        if (countries is null || !countries.Any())
            throw new DomainException("Country records cannot be null or empty.");

        return countries
            .OrderByDescending(country => country.PopulationDensity)
            .First();
    }
}