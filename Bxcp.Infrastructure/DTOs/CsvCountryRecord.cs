namespace Bxcp.Infrastructure.DTOs;

/// <summary>
/// Data Transfer Object representing a single row from the countries CSV file
/// </summary>
public record CsvCountryRecord
{
    public required string Name { get; init; }
    public required string Capital { get; init; }
    public required string Accession { get; init; }
    public required int Population { get; init; }
    public required double Area { get; init; }
    public required string GDP { get; init; }
    public required string HDI { get; init; }
    public required string MEPs { get; init; }
}