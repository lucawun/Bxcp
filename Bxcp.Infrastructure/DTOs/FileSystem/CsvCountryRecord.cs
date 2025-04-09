namespace Bxcp.Infrastructure.DTOs.FileSystem;

/// <summary>
/// Data Transfer Object representing a single row from the countries CSV file
/// </summary>
public record CsvCountryRecord
{
    public string Name { get; init; }
    public string Capital { get; init; }
    public string Accession { get; init; }
    public string Population { get; init; }
    public string Area { get; init; }
    public string GDP { get; init; }
    public string HDI { get; init; }
    public string MEPs { get; init; }
}