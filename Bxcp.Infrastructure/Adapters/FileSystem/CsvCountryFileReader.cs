using Bxcp.Application.Ports.Secondary;
using Bxcp.Infrastructure.DTOs;
using Microsoft.Extensions.Logging;
using System.Globalization;


namespace Bxcp.Infrastructure.Adapters.FileSystem;

/// <summary>
/// Implementation of the file reader for country data CSV files
/// </summary>
public class CsvCountryFileReader : IFileReader<CountryCsvRecord>
{
    private readonly ILogger<CsvCountryFileReader> _logger;

    public CsvCountryFileReader(ILogger<CsvCountryFileReader> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Reads all country records from the specified CSV file
    /// </summary>
    /// <param name="filePath">Path to the countries CSV file</param>
    /// <returns>Collection of country CSV records</returns>
    public IEnumerable<CountryCsvRecord> ReadAllRecords(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            _logger.LogError("Countries CSV file not found at path: {FilePath}", filePath);
            throw new FileNotFoundException($"Countries CSV file not found at path: {filePath}");
        }

        _logger.LogInformation("Reading country data from file: {FilePath}", filePath);

        var records = new List<CountryCsvRecord>();
        var lines = File.ReadAllLines(filePath);

        // Skip header row
        for (int i = 1; i < lines.Length; i++)
        {
            try
            {
                var record = ParseCountryLine(lines[i]);
                if (record != null)
                {
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to parse line {LineNumber}: {Error}", i, ex.Message);
            }
        }

        _logger.LogInformation("Successfully read {RecordCount} country records from file", records.Count);
        return records;
    }

    private CountryCsvRecord ParseCountryLine(string line)
    {
        if (string.IsNullOrWhiteSpace(line))
        {
            return null;
        }

        // Split by semicolon, accounting for possible quoted values
        var values = line.Split(';')
            .Select(v => v.Trim())
            .ToArray();

        if (values.Length < 8)
        {
            _logger.LogWarning("Line has insufficient values: {Line}", line);
            return null;
        }

        return new CountryCsvRecord
        {
            Name = values[0],
            Capital = values[1],
            Accession = values[2],
            Population = values[3],
            Area = values[4],
            GDP = values[5],
            HDI = values[6],
            MEPs = values[7]
        };
    }
}