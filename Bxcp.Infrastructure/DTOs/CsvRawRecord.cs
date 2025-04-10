using System.Globalization;

namespace Bxcp.Infrastructure.DTOs;

/// <summary>
/// Helper class to extract typed values from CSV data
/// </summary>
internal class CsvRawRecord
{
    private readonly string[] _values;
    private readonly Dictionary<string, int> _columnMap;

    internal CsvRawRecord(string[] values, Dictionary<string, int> columnMap)
    {
        _values = values;
        _columnMap = columnMap;
    }

    /// <summary>
    /// Gets a string value for the specified column
    /// </summary>
    public string GetString(string columnName)
    {
        if (_columnMap.TryGetValue(columnName, out int index) && index < _values.Length)
        {
            return _values[index];
        }
        return string.Empty;
    }

    /// <summary>
    /// Gets an integer value for the specified column
    /// </summary>
    public int GetInt(string columnName)
    {
        string value = GetString(columnName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0;
        }

        // Remove thousand separators
        value = value.Replace(",", "");
        return int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out int result)
            ? result
            : 0;
    }

    /// <summary>
    /// Gets a double value for the specified column
    /// </summary>
    public double GetDouble(string columnName)
    {
        string value = GetString(columnName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0.0;
        }

        // Replace comma with dot for parsing with invariant culture
        value = value.Replace(',', '.');
        return double.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out double result)
            ? result
            : 0.0;
    }
}