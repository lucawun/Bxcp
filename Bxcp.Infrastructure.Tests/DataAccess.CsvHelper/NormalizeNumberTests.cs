using Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;
using Xunit;

namespace Bxcp.Infrastructure.Tests.DataAccess.CsvHelper;

public class NormalizeNumberTests
{
    [Theory]
    [InlineData("1.234.567,89", "1234567.89")]  // European format
    [InlineData("1,234,567.89", "1234567.89")]  // US format
    [InlineData("1234.56", "1234.56")]         // Standard decimal
    [InlineData("1234,56", "1234.56")]         // Comma as decimal separator
    [InlineData("", "")]                       // Empty string
    [InlineData(null, null)]                        // Null input, empty string expected
    [InlineData("1234", "1234")]               // Integer
    public void NormalizeHandlesVariousFormatsCorrectly(string? input, string? expected)
    {
        // Act
        string result = NormalizeNumber.Normalize(input!);

        // Assert
        Assert.Equal(expected, result);
    }
}
