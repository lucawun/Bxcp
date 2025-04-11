using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Bxcp.Infrastructure.DataAccess.CsvHelper;
using Bxcp.Infrastructure.DTOs;

namespace Bxcp.Infrastructure.Adapters;

/// <summary>
/// Repository that reads country data from CSV files and converts to domain models
/// </summary>
public class CsvCountryRepository : IDataProviderRepository<Country>
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
        IEnumerable<CsvCountryRecord> csvRecords = _fileReader.ReadAllRecords();
        return csvRecords.Select(MapToDomainEntity);
    }

    /// <summary>
    /// Maps a CSV record to a domain entity
    /// </summary>
    private static Country MapToDomainEntity(CsvCountryRecord record) => new(record.Name, record.Population, record.Area);

}