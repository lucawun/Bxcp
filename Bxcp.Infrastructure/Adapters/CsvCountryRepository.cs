using Bxcp.Domain.Models;
using Bxcp.Domain.Repositories;
using Bxcp.Infrastructure.DataAccess;
using Bxcp.Infrastructure.DTOs;
using System.Globalization;

namespace Bxcp.Infrastructure.Adapters;

/// <summary>
/// Repository that reads country data from CSV files and converts to domain models
/// </summary>
public class CsvCountryRepository : IRepository<Country>
{
    private readonly CsvCountryFileReader _fileReader;

    public CsvCountryRepository(CsvCountryFileReader fileReader)
    {
        _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
    }

    /// <summary>
    /// Reads all country records from CSV and maps them to domain models
    /// </summary>
    public IEnumerable<Country> ReadAllRecords()
    {
        var csvRecords = _fileReader.ReadAllRecords();
        return csvRecords.Select(MapToDomainEntity);
    }

    /// <summary>
    /// Maps a CSV record to a domain entity
    /// </summary>
    private static Country MapToDomainEntity(CsvCountryRecord record) => new()
    {
        Name = record.Name,
        Population = record.Population,
        Area = record.Area
    };
}