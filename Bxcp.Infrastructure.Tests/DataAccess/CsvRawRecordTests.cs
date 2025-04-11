using Bxcp.Infrastructure.DTOs;
using Xunit;

namespace Bxcp.Infrastructure.Tests.DataAccess;

public class CsvRawRecordTests
{
    [Fact]
    public void GetString_ExistingColumn_ReturnsValue()
    {
        // Arrange
        string[] values = new[] { "Value1", "Value2", "Value3" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        string result = record.GetString("Column2");

        // Assert
        Assert.Equal("Value2", result);
    }

    [Fact]
    public void GetString_NonExistingColumn_ReturnsEmptyString()
    {
        // Arrange
        string[] values = new[] { "Value1", "Value2", "Value3" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        string result = record.GetString("NonExistingColumn");

        // Assert
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public void GetString_CaseInsensitiveColumn_ReturnsValue()
    {
        // Arrange
        string[] values = new[] { "Value1", "Value2", "Value3" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        string result = record.GetString("column2");

        // Assert
        Assert.Equal("Value2", result);
    }

    [Fact]
    public void GetInt_ValidIntValue_ReturnsIntValue()
    {
        // Arrange
        string[] values = new[] { "123", "456", "789" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        int result = record.GetInt("Column1");

        // Assert
        Assert.Equal(123, result);
    }

    [Fact]
    public void GetInt_ValueWithThousandSeparator_ReturnsIntValue()
    {
        // Arrange
        string[] values = new[] { "1,234", "456", "789" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        int result = record.GetInt("Column1");

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetInt_NonNumericValue_ThrowsFormatException()
    {
        // Arrange
        string[] values = new[] { "abc", "456", "789" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
    {
        { "Column1", 0 },
        { "Column2", 1 },
        { "Column3", 2 }
    };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act & Assert
        FormatException exception = Assert.Throws<System.FormatException>(() => record.GetInt("Column1"));
        Assert.Contains("Column 'Column1' contains invalid integer format: 'abc'", exception.Message);
    }

    [Fact]
    public void GetInt_EmptyValue_ReturnsZero()
    {
        // Arrange
        string[] values = new[] { "", "456", "789" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        int result = record.GetInt("Column1");

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetDouble_ValidDoubleValue_ReturnsDoubleValue()
    {
        // Arrange
        string[] values = new[] { "123.45", "456.78", "789.01" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        double result = record.GetDouble("Column1");

        // Assert
        Assert.Equal(123.45, result);
    }

    [Fact]
    public void GetDouble_ValueWithCommaDecimalSeparator_ReturnsDoubleValue()
    {
        // Arrange
        string[] values = new[] { "123,45", "456,78", "789,01" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        double result = record.GetDouble("Column1");

        // Assert
        Assert.Equal(123.45, result);
    }

    [Fact]
    public void GetDouble_NonNumericValue_ReturnsZero()
    {
        // Arrange
        string[] values = new[] { "abc", "456.78", "789.01" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act & Assert
        FormatException exception = Assert.Throws<FormatException>(() => record.GetDouble("Column1"));
        Assert.Contains("Column 'Column1' contains invalid decimal format: 'abc'", exception.Message);
    }

    [Fact]
    public void GetDouble_EmptyValue_ReturnsZero()
    {
        // Arrange
        string[] values = new[] { "", "456.78", "789.01" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        double result = record.GetDouble("Column1");

        // Assert
        Assert.Equal(0.0, result);
    }

    [Fact]
    public void GetDouble_NonExistingColumn_ReturnsZero()
    {
        // Arrange
        string[] values = new[] { "123.45", "456.78", "789.01" };
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Column1", 0 },
            { "Column2", 1 },
            { "Column3", 2 }
        };
        CsvRawRecord record = new CsvRawRecord(values, columnMap);

        // Act
        double result = record.GetDouble("NonExistingColumn");

        // Assert
        Assert.Equal(0.0, result);
    }
}