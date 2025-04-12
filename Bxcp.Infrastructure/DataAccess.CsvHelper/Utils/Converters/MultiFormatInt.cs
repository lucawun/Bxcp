using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

/// <summary>
/// Converter that handles multiple number formats for integers
/// </summary>
public class MultiFormatInt : Int32Converter
{
    private const int DEFAULT_VALUE = 0;
    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0;

        string normalizedText = NormalizeNumber.Normalize(text);

        // Try to parse as double first (in case it has decimals), then convert to int
        if (double.TryParse(normalizedText, NumberStyles.Any, CultureInfo.InvariantCulture, out double doubleValue))
        {
            return Convert.ToInt32(doubleValue);
        }

        // Fall back to the default converter if our custom logic fails
        return base.ConvertFromString(text, row, memberMapData) ?? DEFAULT_VALUE;
    }
}