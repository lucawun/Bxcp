using System.Globalization;
using System.Text.RegularExpressions;

namespace Bxcp.Infrastructure.DTOs;

/// <summary>
/// Helper class to extract typed values from CSV data
/// </summary>
public class CsvRawRecord
{
    private readonly string[] _values;
    private readonly Dictionary<string, int> _columnMap;
    private readonly CultureInfo _primaryCulture;
    private readonly CultureInfo[] _fallbackCultures;
    private static readonly Regex _currencySymbolRegex = new(@"[^\d\s\.,\-\+]", RegexOptions.Compiled);

    /// <summary>
    /// Creates a new instance of CsvRawRecord
    /// </summary>
    /// <param name="values">Array of CSV values</param>
    /// <param name="columnMap">Dictionary mapping column names to array indices</param>
    /// <param name="primaryCulture">Primary culture to use for parsing (default: de-DE)</param>
    /// <param name="fallbackCultures">Optional fallback cultures to try if parsing with primary culture fails</param>
    public CsvRawRecord(string[] values, Dictionary<string, int> columnMap,
                       CultureInfo primaryCulture = null,
                       CultureInfo[] fallbackCultures = null)
    {
        _values = values;
        _columnMap = columnMap;
        _primaryCulture = primaryCulture ?? CultureInfo.GetCultureInfo("de-DE");
        _fallbackCultures = fallbackCultures ?? new[] {
            CultureInfo.InvariantCulture,
            CultureInfo.GetCultureInfo("en-US")
        };
    }

    /// <summary>
    /// Gets a string value for the specified column
    /// </summary>
    public string GetString(string columnName)
    {
        if (_columnMap.TryGetValue(columnName, out int index) && index < _values.Length)
        {
            return _values[index]?.Trim() ?? string.Empty;
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

        // First, check if it might be a decimal value mistakenly provided for an int field
        if (value.Contains('.') || value.Contains(','))
        {
            // Try to parse as double first, then convert to int
            try
            {
                double doubleValue = GetDouble(columnName);
                return Convert.ToInt32(doubleValue);
            }
            catch
            {
                // Continue with normal int parsing if that fails
            }
        }

        // Try with primary culture first
        if (int.TryParse(value, NumberStyles.Any, _primaryCulture, out int result))
        {
            return result;
        }

        // Try with each fallback culture
        foreach (var culture in _fallbackCultures)
        {
            if (int.TryParse(value, NumberStyles.Any, culture, out result))
            {
                return result;
            }
        }

        // Try with some common pre-processing
        string preprocessed = PreprocessNumericString(value);
        if (int.TryParse(preprocessed, NumberStyles.Any, CultureInfo.InvariantCulture, out result))
        {
            return result;
        }

        // Try a more aggressive approach by removing all non-digit characters
        string digitsOnly = new string(value.Where(char.IsDigit).ToArray());
        if (int.TryParse(digitsOnly, out result))
        {
            return result;
        }

        throw new FormatException($"Column '{columnName}' contains invalid integer format: '{value}'");
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

        // Handle specific case: if the value has only one decimal point and no commas
        // Make sure it's properly interpreted when the primary culture uses comma for decimals
        if (value.Contains('.') && !value.Contains(',') && value.Count(c => c == '.') == 1 &&
            _primaryCulture.NumberFormat.NumberDecimalSeparator == ",")
        {
            // Convert to the format expected by the primary culture
            string adaptedValue = value.Replace('.', ',');
            if (double.TryParse(adaptedValue, NumberStyles.Any, _primaryCulture, out double result))
            {
                return result;
            }
        }

        // Try with primary culture first
        if (double.TryParse(value, NumberStyles.Any, _primaryCulture, out double resultPrimary))
        {
            return resultPrimary;
        }

        // Try with each fallback culture
        foreach (var culture in _fallbackCultures)
        {
            if (double.TryParse(value, NumberStyles.Any, culture, out double resultFallback))
            {
                return resultFallback;
            }
        }

        // Special case for values like "10.5" that might be interpreted as "105"
        if (value.Contains('.') && double.TryParse(value.Replace('.', ','), NumberStyles.Any, _primaryCulture, out double dotToCommaResult))
        {
            return dotToCommaResult;
        }

        // Try with some common pre-processing
        string preprocessed = PreprocessNumericString(value);
        if (double.TryParse(preprocessed, NumberStyles.Any, CultureInfo.InvariantCulture, out double resultPreprocessed))
        {
            return resultPreprocessed;
        }

        // Last resort: try all specific cultures
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            if (double.TryParse(value, NumberStyles.Any, culture, out double resultAllCultures))
            {
                return resultAllCultures;
            }
        }

        throw new FormatException($"Column '{columnName}' contains invalid decimal format: '{value}'");
    }

    /// <summary>
    /// Gets a decimal value for the specified column
    /// </summary>
    public decimal GetDecimal(string columnName)
    {
        string value = GetString(columnName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return 0m;
        }

        // Handle specific case: if the value has only one decimal point and no commas
        // Make sure it's properly interpreted when the primary culture uses comma for decimals
        if (value.Contains('.') && !value.Contains(',') && value.Count(c => c == '.') == 1 &&
            _primaryCulture.NumberFormat.NumberDecimalSeparator == ",")
        {
            // Convert to the format expected by the primary culture
            string adaptedValue = value.Replace('.', ',');
            if (decimal.TryParse(adaptedValue, NumberStyles.Any, _primaryCulture, out decimal result))
            {
                return result;
            }
        }

        // Try with primary culture first
        if (decimal.TryParse(value, NumberStyles.Any, _primaryCulture, out decimal resultPrimary))
        {
            return resultPrimary;
        }

        // Try with each fallback culture
        foreach (var culture in _fallbackCultures)
        {
            if (decimal.TryParse(value, NumberStyles.Any, culture, out decimal resultFallback))
            {
                return resultFallback;
            }
        }

        // Special case for values like "10.5" that might be interpreted as "105"
        if (value.Contains('.') && decimal.TryParse(value.Replace('.', ','), NumberStyles.Any, _primaryCulture, out decimal dotToCommaResult))
        {
            return dotToCommaResult;
        }

        // Try with some common pre-processing
        string preprocessed = PreprocessNumericString(value);
        if (decimal.TryParse(preprocessed, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal resultPreprocessed))
        {
            return resultPreprocessed;
        }

        // Last resort: try all specific cultures
        foreach (var culture in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            if (decimal.TryParse(value, NumberStyles.Any, culture, out decimal resultAllCultures))
            {
                return resultAllCultures;
            }
        }

        throw new FormatException($"Column '{columnName}' contains invalid decimal format: '{value}'");
    }

    /// <summary>
    /// Gets a nullable decimal value for the specified column
    /// </summary>
    public decimal? GetNullableDecimal(string columnName)
    {
        string value = GetString(columnName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        try
        {
            return GetDecimal(columnName);
        }
        catch (FormatException)
        {
            return null;
        }
    }

    /// <summary>
    /// Gets a DateTime value for the specified column
    /// </summary>
    public DateTime? GetDateTime(string columnName, string[] formatStrings = null)
    {
        string value = GetString(columnName);
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        // Default date formats to try
        formatStrings = formatStrings ?? new[]
        {
            "yyyy-MM-dd", "dd.MM.yyyy", "MM/dd/yyyy",
            "yyyy-MM-dd HH:mm:ss", "dd.MM.yyyy HH:mm:ss", "MM/dd/yyyy HH:mm:ss"
        };

        // Try parsing with primary culture and specified formats
        foreach (var format in formatStrings)
        {
            if (DateTime.TryParseExact(value, format, _primaryCulture,
                                      DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        // Try with fallback cultures
        foreach (var culture in _fallbackCultures)
        {
            foreach (var format in formatStrings)
            {
                if (DateTime.TryParseExact(value, format, culture,
                                          DateTimeStyles.None, out DateTime result))
                {
                    return result;
                }
            }
        }

        // Try standard parsing with various cultures
        if (DateTime.TryParse(value, _primaryCulture, DateTimeStyles.None, out DateTime primaryResult))
        {
            return primaryResult;
        }

        foreach (var culture in _fallbackCultures)
        {
            if (DateTime.TryParse(value, culture, DateTimeStyles.None, out DateTime result))
            {
                return result;
            }
        }

        return null;
    }

    /// <summary>
    /// Preprocesses numeric strings to handle common formatting issues
    /// </summary>
    private string PreprocessNumericString(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return value;

        // Remove currency symbols, percent signs, and other non-numeric characters
        value = _currencySymbolRegex.Replace(value, "").Trim();

        // Count occurrences of dots and commas
        int dotCount = value.Count(c => c == '.');
        int commaCount = value.Count(c => c == ',');

        // Special case for single dot decimal separator (10.5)
        if (dotCount == 1 && commaCount == 0)
        {
            // If the primary culture uses comma as decimal separator, replace the dot with a comma
            if (_primaryCulture.NumberFormat.NumberDecimalSeparator == ",")
            {
                return value.Replace(".", ",");
            }
            // Otherwise, leave as is (already in correct format for cultures using dot)
            return value;
        }

        // Handle common European number format with thousands separators (1.234.567,89)
        if (dotCount > 0 && commaCount == 1 && value.LastIndexOf(',') > value.LastIndexOf('.'))
        {
            // Replace all dots, and then replace the comma with a dot
            return value.Replace(".", "").Replace(",", ".");
        }
        // Handle common US/UK number format with thousands separators (1,234,567.89)
        else if (commaCount > 0 && dotCount == 1 && value.LastIndexOf('.') > value.LastIndexOf(','))
        {
            // Remove all commas
            return value.Replace(",", "");
        }
        // Handle European format without thousands separators (1234567,89)
        else if (dotCount == 0 && commaCount == 1)
        {
            // Replace comma with dot
            return value.Replace(",", ".");
        }
        // Handle multiple dots without commas (e.g., "1.234.567")
        else if (dotCount > 1 && commaCount == 0)
        {
            // Assume European format with dots as thousands separators
            return value.Replace(".", "");
        }
        // Handle multiple commas without dots (e.g., "1,234,567")
        else if (commaCount > 1 && dotCount == 0)
        {
            // Assume US format with commas as thousands separators
            return value.Replace(",", "");
        }
        // Handle complex cases with multiple dots and commas
        else if (dotCount > 0 && commaCount > 0)
        {
            // Compare the last positions of dots and commas to determine format
            if (value.LastIndexOf(',') > value.LastIndexOf('.'))
            {
                // Likely European format (1.234.567,89)
                return value.Replace(".", "").Replace(",", ".");
            }
            else
            {
                // Likely US format (1,234,567.89)
                return value.Replace(",", "");
            }
        }

        return value;
    }

    /// <summary>
    /// Gets a boolean value for the specified column
    /// </summary>
    public bool GetBoolean(string columnName)
    {
        string value = GetString(columnName).ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(value))
            return false;

        return value switch
        {
            "true" or "yes" or "ja" or "oui" or "si" or "1" or "y" or "j" or "wahr" => true,
            _ => false
        };
    }

    /// <summary>
    /// Checks if the specified column exists
    /// </summary>
    public bool HasColumn(string columnName)
    {
        return _columnMap.ContainsKey(columnName);
    }

    /// <summary>
    /// Gets all values as an array of strings
    /// </summary>
    public string[] GetAllValues()
    {
        return _values;
    }

    /// <summary><
    /// Gets the column mapping dictionary
    /// </summary>
    public IReadOnlyDictionary<string, int> GetColumnMap()
    {
        return _columnMap;
    }
}