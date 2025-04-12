using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

/// <summary>
/// Converter that handles multiple number formats for doubles
/// </summary>
public class MultiFormatDouble : DoubleConverter
{
    private const double DEFAULT_VALUE = 0.0;

    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0.0;

        string normalizedText = NormalizeNumber.Normalize(text);

        // Try to parse with the normalized format
        if (double.TryParse(normalizedText, NumberStyles.Any, CultureInfo.InvariantCulture, out double result))
        {
            return result;
        }

        // Fall back to the default converter if our custom logic fails
        return base.ConvertFromString(text, row, memberMapData) ?? DEFAULT_VALUE;
    }
}