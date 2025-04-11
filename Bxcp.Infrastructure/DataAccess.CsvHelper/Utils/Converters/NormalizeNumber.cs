namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

/// <summary>
/// Utility class for handling number format normalization
/// </summary>
public static class NormalizeNumber
{
    public static string Normalize(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return text;

        // Check if the text matches the European format pattern (e.g., "1.234.567,89")
        bool isEuropeanFormat = text.Contains(".") && text.Contains(",") &&
            text.LastIndexOf(",") > text.LastIndexOf(".");

        // Check if the text matches the US format pattern (e.g., "1,234,567.89")
        bool isUSFormat = text.Contains(",") && text.Contains(".") &&
            text.LastIndexOf(".") > text.LastIndexOf(",");

        // For single separator cases
        bool hasSingleDot = text.Contains(".") && !text.Contains(",");
        bool hasSingleComma = text.Contains(",") && !text.Contains(".");

        if (isEuropeanFormat)
        {
            // European format: replace dots (thousand separators) and convert comma to dot
            return text.Replace(".", "").Replace(",", ".");
        }
        else if (isUSFormat)
        {
            // US format: just remove the commas
            return text.Replace(",", "");
        }
        else if (hasSingleComma)
        {
            // If only a comma is present, treat it as a decimal separator
            return text.Replace(",", ".");
        }

        // For hasSingleDot or any other format, return as is (dot as decimal separator is standard for InvariantCulture)
        return text;
    }
}