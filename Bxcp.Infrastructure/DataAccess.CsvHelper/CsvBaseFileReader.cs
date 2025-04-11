using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using CsvHelper;

using System.Globalization;
using Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

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

    private CsvConfiguration CreateConfiguration(char delimiter)
    {
        return new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            Delimiter = delimiter.ToString(),
            HasHeaderRecord = true,
            MissingFieldFound = null,  // Ignore missing fields
            TrimOptions = TrimOptions.Trim,
            PrepareHeaderForMatch = args => args.Header.ToLower() // Case-insensitive header matching
        };
    }

    /// <summary>
    /// Reads all records from the CSV file
    /// </summary>
    public virtual IEnumerable<T> ReadAllRecords()
    {
        EnsureFileExists();

        using var reader = new StreamReader(_filePath);
        using var csv = new CsvReader(reader, _configuration);

        RegisterConverters(csv);
        RegisterMapping(csv);

        return csv.GetRecords<T>().ToList();
    }

    private void RegisterConverters(CsvReader csv)
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
    protected ITypeConverter CreateSplitFieldConverter<TField>(int partIndex, string separator = " ")
    {
        return new SplitField<TField>(partIndex, separator);
    }
}