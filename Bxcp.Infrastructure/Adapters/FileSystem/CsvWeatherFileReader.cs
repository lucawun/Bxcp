using Bxcp.Application.Ports.Secondary;
using Bxcp.Infrastructure.DTOs;
using Microsoft.Extensions.Logging;
using System.Globalization;

namespace Bxcp.Infrastructure.Adapters.FileSystem;

/// <summary>
/// Implementation of the file reader for weather data CSV files
/// </summary>
public class CsvWeatherFileReader : IFileReader<WeatherCsvRecord>
{
    private readonly ILogger<CsvWeatherFileReader> _logger;
    private readonly char _delimiter = ',';

    public CsvWeatherFileReader(ILogger<CsvWeatherFileReader> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Reads all weather records from the specified CSV file
    /// </summary>
    /// <param name="filePath">Path to the weather CSV file</param>
    /// <returns>Collection of weather CSV records</returns>
    public IEnumerable<WeatherCsvRecord> ReadAllRecords(string filePath)
    {
        ValidateFilePath(filePath);

        _logger.LogInformation("Reading weather data from file: {FilePath}", filePath);

        try
        {
            string[] lines = File.ReadAllLines(filePath);

            if (lines.Length <= 1)
            {
                _logger.LogWarning("Weather CSV file is empty or contains only headers: {FilePath}", filePath);
                return Enumerable.Empty<WeatherCsvRecord>();
            }

            string headerLine = lines.First();
            Dictionary<string, int> columnMap = CreateColumnMap(headerLine);

            return ParseWeatherRecords(lines.Skip(1), columnMap);
        }
        catch (Exception ex) when (ex is not FileNotFoundException)
        {
            _logger.LogError(ex, "Error reading weather data from file: {FilePath}", filePath);
            throw new InvalidOperationException($"Failed to read weather data from file: {ex.Message}", ex);
        }
    }

    private void ValidateFilePath(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(filePath));
        }

        if (!File.Exists(filePath))
        {
            _logger.LogError("Weather CSV file not found at path: {FilePath}", filePath);
            throw new FileNotFoundException($"Weather CSV file not found at path: {filePath}");
        }
    }

    private Dictionary<string, int> CreateColumnMap(string headerLine)
    {
        string[] headerValues = SplitCsvLine(headerLine);
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < headerValues.Length; i++)
        {
            string header = headerValues[i].Trim();
            if (!string.IsNullOrWhiteSpace(header))
            {
                columnMap[header] = i;
            }
        }

        EnsureRequiredColumnsExist(columnMap);

        return columnMap;
    }

    private void EnsureRequiredColumnsExist(Dictionary<string, int> columnMap)
    {
        string[] requiredColumns = { "Day", "MxT", "MnT" };

        foreach (string column in requiredColumns)
        {
            if (!columnMap.ContainsKey(column))
            {
                throw new InvalidOperationException($"Required column '{column}' not found in CSV header");
            }
        }
    }

    private IEnumerable<WeatherCsvRecord> ParseWeatherRecords(IEnumerable<string> dataLines, Dictionary<string, int> columnMap)
    {
        List<WeatherCsvRecord> records = new List<WeatherCsvRecord>();
        int lineNumber = 1; // Header is line 0

        foreach (string line in dataLines)
        {
            lineNumber++;

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                WeatherCsvRecord record = ParseWeatherLine(line, columnMap);
                records.Add(record);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to parse line {LineNumber}: {Error}", lineNumber, ex.Message);
            }
        }

        _logger.LogInformation("Successfully read {RecordCount} weather records", records.Count);
        return records;
    }

    private WeatherCsvRecord ParseWeatherLine(string line, Dictionary<string, int> columnMap)
    {
        string[] values = SplitCsvLine(line);

        if (values.Length <= columnMap.Values.Max())
        {
            throw new FormatException($"Line has insufficient values: {line}");
        }

        return new WeatherCsvRecord
        {
            Day = GetIntValue(values, columnMap, "Day"),
            MxT = GetDoubleValue(values, columnMap, "MxT"),
            MnT = GetDoubleValue(values, columnMap, "MnT"),
            AvT = GetDoubleValue(values, columnMap, "AvT"),
            AvDP = GetDoubleValue(values, columnMap, "AvDP"),
            TPcpn = GetDoubleValue(values, columnMap, "1HrP TPcpn", "TPcpn"),
            PDir = GetIntValue(values, columnMap, "PDir"),
            AvSp = GetDoubleValue(values, columnMap, "AvSp"),
            Dir = GetIntValue(values, columnMap, "Dir"),
            MxS = GetIntValue(values, columnMap, "MxS"),
            SkyC = GetDoubleValue(values, columnMap, "SkyC"),
            MxR = GetIntValue(values, columnMap, "MxR"),
            Mn = GetIntValue(values, columnMap, "Mn"),
            R = GetDoubleValue(values, columnMap, "R", "R AvSLP"),
            AvSLP = GetDoubleValue(values, columnMap, "AvSLP", "R AvSLP")
        };
    }

    private string[] SplitCsvLine(string line)
    {
        // This is a simplified CSV parsing logic
        // For production code, consider using a dedicated CSV parsing library
        return line.Split(_delimiter)
            .Select(v => v.Trim())
            .ToArray();
    }

    private int GetIntValue(string[] values, Dictionary<string, int> columnMap, string columnName, string alternateColumnName = null)
    {
        int index = GetColumnIndex(columnMap, columnName, alternateColumnName);
        return index >= 0 && index < values.Length
            ? TryParseInt(values[index])
            : 0;
    }

    private double GetDoubleValue(string[] values, Dictionary<string, int> columnMap, string columnName, string alternateColumnName = null)
    {
        int index = GetColumnIndex(columnMap, columnName, alternateColumnName);
        return index >= 0 && index < values.Length
            ? TryParseDouble(values[index])
            : 0.0;
    }

    private int GetColumnIndex(Dictionary<string, int> columnMap, string columnName, string alternateColumnName)
    {
        if (columnMap.TryGetValue(columnName, out int index))
        {
            return index;
        }

        if (alternateColumnName != null && columnMap.TryGetValue(alternateColumnName, out index))
        {
            return index;
        }

        // If column is optional, return -1
        return -1;
    }

    private int TryParseInt(string value)
    {
        return int.TryParse(value, out int result) ? result : 0;
    }

    private double TryParseDouble(string value)
    {
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result)
            ? result
            : 0.0;
    }
}