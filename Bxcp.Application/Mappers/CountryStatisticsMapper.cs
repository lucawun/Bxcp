using Bxcp.Application.DTOs;
using Bxcp.Domain.Models;

namespace Bxcp.Application.Mappers;

/// <summary>
/// Maps between domain models and DTOs for country use case
/// </summary>
public static class CountryStatisticsMapper
{
    /// <summary>
    /// Maps a domain model to a DTO
    /// </summary>
    /// <param name="countryRecord">The domain model to map</param>
    /// <returns>A DTO representing the country analysis result</returns>
    public static CountryAnalysisResult ToCountryStatisticsResult(Country countryRecord)
    {
        ArgumentNullException.ThrowIfNull(countryRecord);

        return new CountryAnalysisResult
        {
            CountryWithHighestDensity = countryRecord.Name,
            HighestDensity = countryRecord.PopulationDensity
        };
    }
}