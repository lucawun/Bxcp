using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Globalization;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

/// <summary>
/// Converter that handles multiple number formats for decimals
/// </summary>
public class MultiFormatDecimal : DecimalConverter
{
    private const decimal DEFAULT_VALUE = 0m;

    public override object ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0m;

        string normalizedText = NormalizeNumber.Normalize(text);

        // Try to parse with the normalized format
        if (decimal.TryParse(normalizedText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }

        return base.ConvertFromString(text, row, memberMapData) ?? DEFAULT_VALUE;
    }
}