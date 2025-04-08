namespace Bxcp.Domain.Models;

/// <summary>
/// Represents data about a country.
/// </summary>
public record CountryRecord
{
    public string Name { get; init; } = string.Empty;
    public string Capital { get; init; } = string.Empty;
    public string AccessionYear { get; init; } = string.Empty;
    public int Population { get; init; }
    public double Area { get; init; }
    public decimal GdpInMillions { get; init; }
    public double HumanDevelopmentIndex { get; init; }
    public int EuropeanParliamentSeats { get; init; }

    /// <summary>
    /// Calculates the population density (people per square kilometer).
    /// </summary>
    public double PopulationDensity => Population / Area;
}