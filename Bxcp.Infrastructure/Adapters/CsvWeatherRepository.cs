using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Bxcp.Infrastructure.DataAccess.CsvHelper;
using Bxcp.Infrastructure.DTOs;

namespace Bxcp.Infrastructure.Adapters;

/// <summary>
/// Repository that reads weather data from CSV files and converts to domain models
/// </summary>
public class CsvWeatherRepository : IDataProviderRepository<Weather>
{
    private readonly CsvWeatherFileReader _fileReader;

    /// <summary>
    /// Initializes a new instance of the CsvWeatherRepository
    /// </summary>
    /// <param name="fileReader">The file reader for accessing weather CSV data</param>
    public CsvWeatherRepository(CsvWeatherFileReader fileReader)
    {
        _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
    }

    /// <summary>
    /// Reads all weather records from CSV and maps them to domain models
    /// </summary>
    /// <returns>Collection of Weather domain entities</returns>
    public IEnumerable<Weather> ReadAllRecords()
    {
        IEnumerable<CsvWeatherRecord> csvRecords = _fileReader.ReadAllRecords();
        return csvRecords.Select(MapToDomainEntity);
    }

    /// <summary>
    /// Maps a CSV record to a domain entity
    /// </summary>
    /// <param name="record">The CSV record to map</param>
    /// <returns>A domain Weather entity</returns>
    private static Weather MapToDomainEntity(CsvWeatherRecord record) => new(record.Day, record.MxT, record.MnT);

}