﻿using Bxcp.Domain.Models;

namespace Bxcp.Domain.DomainServices.Ports;

public interface ICountryStatisticsService
{
    /// <summary>
    /// Finds the country with the highest population density.
    /// </summary>
    Country FindHighestPopulationDensity(IEnumerable<Country> countries);
}