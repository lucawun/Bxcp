using Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper;

/// <summary>
/// Base class for reading CSV files with header-based mapping
/// </summary>
public abstract class CsvBaseFileReader<T>
{
    private readonly CsvConfiguration _configuration;
    private readonly string _filePath;

    protected CsvBaseFileReader(string filePath, char delimiter)
    {
        if (string.IsNullOrEmpty(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        _filePath = filePath;
        _configuration = CreateConfiguration(delimiter);
    }

    private static CsvConfiguration CreateConfiguration(char delimiter) => new(CultureInfo.InvariantCulture)
    {
        Delimiter = delimiter.ToString(),
        HasHeaderRecord = true,
        MissingFieldFound = null,  // Ignore missing fields
        TrimOptions = TrimOptions.Trim,
        PrepareHeaderForMatch = args => args.Header.ToUpperInvariant() // Case-insensitive header matching
    };

    /// <summary>
    /// Reads all records from the CSV file
    /// </summary>
    public virtual IEnumerable<T> ReadAllRecords()
    {
        EnsureFileExists();

        using StreamReader reader = new(_filePath);
        using CsvReader csv = new(reader, _configuration);

        RegisterConverters(csv);
        RegisterMapping(csv);

        return [.. csv.GetRecords<T>()];
    }

    private static void RegisterConverters(CsvReader csv)
    {
        csv.Context.TypeConverterCache.AddConverter<int>(new MultiFormatInt());
        csv.Context.TypeConverterCache.AddConverter<double>(new MultiFormatDouble());
        csv.Context.TypeConverterCache.AddConverter<decimal>(new MultiFormatDecimal());
    }

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
    /// Registers the class map for the CSV reader
    /// </summary>
    protected abstract void RegisterMapping(CsvReader csvReader);

    /// <summary>
    /// Helper method to create a converter for a combined field (e.g., "Value1 Value2")
    /// </summary>
    protected ITypeConverter CreateSplitFieldConverter<TField>(int partIndex, string separator = " ") => new SplitField<TField>(partIndex, separator);
}
