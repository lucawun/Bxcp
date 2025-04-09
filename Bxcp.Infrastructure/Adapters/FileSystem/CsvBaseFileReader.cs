using Bxcp.Application.Ports.Outgoing;

namespace Bxcp.Infrastructure.Adapters.FileSystem;

/// <summary>
/// Base class for reading CSV files with header-based mapping
/// </summary>
public abstract class BaseCsvFileReader<T> : IRepository<T> 
{
    private readonly char _delimiter;
    private readonly string _filePath;

    protected BaseCsvFileReader(string filePath, char delimiter)
    {
        _filePath = filePath;
        _delimiter = delimiter;
    }

    /// <summary>
    /// Reads all records from the CSV file
    /// </summary>
    public IEnumerable<T> ReadAllRecords()
    {
        EnsureFileExists();

        string[] lines = File.ReadAllLines(_filePath);
        if (lines.Length <= 1)
        {
            return Enumerable.Empty<T>();
        }

        string headerLine = lines[0];
        Dictionary<string, int> columnMap = MapColumns(headerLine);
        EnsureRequiredColumnsExist(columnMap);

        return ParseDataLines(lines.Skip(1).ToArray(), columnMap);
    }


    /// <summary>
    /// Ensures that the file exists and is accessible
    /// </summary>
    private void EnsureFileExists()
    {
        if (string.IsNullOrWhiteSpace(_filePath))
        {
            throw new ArgumentException("File path cannot be null or empty", nameof(_filePath));
        }

        if (!File.Exists(_filePath))
        {
            throw new FileNotFoundException($"CSV file not found: {_filePath}");
        }
    }

    /// <summary>
    /// Maps the columns in the header line to their respective indices
    /// </summary>
    private Dictionary<string, int> MapColumns(string headerLine)
    {
        string[] headers = SplitLine(headerLine);
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < headers.Length; i++)
        {
            if (!string.IsNullOrWhiteSpace(headers[i]))
            {
                columnMap[headers[i].Trim()] = i;
            }
        }

        return columnMap;
    }

    /// <summary>
    /// Ensures that all required columns exist in the CSV file
    /// </summary>
    private void EnsureRequiredColumnsExist(Dictionary<string, int> columnMap)
    {
        string[] requiredColumns = GetRequiredColumns();
        List<string> missingColumns = requiredColumns
            .Where(column => !columnMap.ContainsKey(column))
            .ToList();

        if (missingColumns.Any())
        {
            throw new InvalidOperationException(
                $"Required columns missing: {string.Join(", ", missingColumns)}");
        }
    }


    /// <summary>
    /// Parses the data lines into records
    /// </summary>
    private IEnumerable<T> ParseDataLines(string[] dataLines, Dictionary<string, int> columnMap)
    {
        List<T> records = new List<T>();

        for (int i = 0; i < dataLines.Length; i++)
        {
            string line = dataLines[i];
            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            try
            {
                string[] values = SplitLine(line);
                CsvRawRecord recordBuilder = new CsvRawRecord(values, columnMap);
                T record = ParseLine(recordBuilder);
                if (record != null)
                {
                    records.Add(record);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error in line {i + 2}: {ex.Message}", ex);
            }
        }

        return records;
    }


    /// <summary>
    /// Splits a line into values based on the delimiter and trims whitespace
    /// </summary>
    private string[] SplitLine(string line)
    {
        return line.Split(_delimiter)
            .Select(value => value.Trim())
            .ToArray();
    }

    /// <summary>
    /// Gets the list of columns that must exist in the CSV file
    /// </summary>
    internal abstract string[] GetRequiredColumns();

    /// <summary>
    /// Parses a CSV line to create a record using the RecordBuilder
    /// </summary>
    internal abstract T ParseLine(CsvRawRecord builder);

}