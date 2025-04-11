using Xunit;

using Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

namespace Bxcp.Infrastructure.Tests.DataAccess.CsvHelper;

public class NormalizeNumberTests
{
    [Theory]
    [InlineData("1.234.567,89", "1234567.89")]  // European format
    [InlineData("1,234,567.89", "1234567.89")]  // US format
    [InlineData("1234.56", "1234.56")]         // Standard decimal
    [InlineData("1234,56", "1234.56")]         // Comma as decimal separator
    [InlineData("", "")]                       // Empty string
    [InlineData(null, null)]                   // Null
    [InlineData("1234", "1234")]               // Integer
    public void Normalize_HandlesVariousFormats_Correctly(string input, string expected)
    {
        // Act
        var result = NormalizeNumber.Normalize(input);

        // Assert
        Assert.Equal(expected, result);
    }
}
