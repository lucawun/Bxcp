using Bxcp.Domain.Exceptions;

namespace Bxcp.Domain.Models;

/// <summary>
/// Represents data about a country.
/// </summary>
public record Country
{
    public string Name { get; init; } = string.Empty;
    public int Population { get; init; }
    public double Area { get; init; }

    /// <summary>
    /// Calculates the population density (people per square kilometer).
    /// </summary>
    public double PopulationDensity => Population / Area;

    /// <summary>
    /// Creates a new Country, validating initial values.
    /// </summary>
    /// <exception cref="DomainException">
    /// Thrown if the name is empty, population is negative, or area is less than or equal to zero.
    /// </exception>
    public Country(string name, int population, double area)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Country name cannot be empty.");

        if (population < 0)
            throw new DomainException("Population cannot be negative.");

        if (area <= 0)
            throw new DomainException("Area must be greater than zero.");

        Name = name;
        Population = population;
        Area = area;
    }
}