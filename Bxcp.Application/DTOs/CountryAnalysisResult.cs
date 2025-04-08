namespace Bxcp.Application.DTOs;

/// <summary>
/// Data Transfer Object containing the results of a country data analysis
/// </summary>
public record CountryAnalysisResult
{
    /// <summary>
    /// The name of the country with the highest population density
    /// </summary>
    public string CountryWithHighestDensity { get; init; } = string.Empty;

    /// <summary>
    /// The value of the highest population density
    /// </summary>
    public double HighestDensity { get; init; }
}