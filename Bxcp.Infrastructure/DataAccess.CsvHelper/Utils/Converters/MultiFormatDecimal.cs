﻿using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using CsvHelper.TypeConversion;


namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

/// <summary>
/// Converter that handles multiple number formats for decimals
/// </summary>
public class MultiFormatDecimal : DecimalConverter
{
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return 0m;

        string normalizedText = NormalizeNumber.Normalize(text);

        // Try to parse with the normalized format
        if (decimal.TryParse(normalizedText, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
        {
            return result;
        }

        // Fall back to the default converter if our custom logic fails
        return base.ConvertFromString(text, row, memberMapData);
    }
}