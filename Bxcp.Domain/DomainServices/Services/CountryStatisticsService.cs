using Bxcp.Domain.DomainServices.Ports;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;

namespace Bxcp.Domain.DomainServices.Services;

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