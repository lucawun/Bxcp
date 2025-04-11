using Bxcp.Infrastructure.DataAccess;
using Bxcp.Infrastructure.DTOs;
using Xunit;

namespace Bxcp.Infrastructure.Tests.DataAccess;

public class CsvCountryFileReaderTests
{
    [Fact]
    public void GetRequiredColumns_ReturnsExpectedColumns()
    {
        // Arrange
        CsvCountryFileReader reader = new CsvCountryFileReader("dummy.csv");

        // Use reflection to call the internal method
        System.Reflection.MethodInfo? method = typeof(CsvCountryFileReader).GetMethod("GetRequiredColumns",
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        // Act
        string[] columns = (string[])method!.Invoke(reader, null)!;

        // Assert
        Assert.Contains("Name", columns);
        Assert.Contains("Capital", columns);
        Assert.Equal(2, columns.Length);
    }

    [Fact]
    public void ParseLine_MapsAllFields()
    {
        // Arrange
        CsvCountryFileReader reader = new CsvCountryFileReader("dummy.csv");
        Dictionary<string, int> columnMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            { "Country", 0 },
            { "City", 1 },
            { "Accession", 2 },
            { "Population", 3 },
            { "Area", 4 },
            { "Density", 5 },
            { "Gini", 6 }
        };

        string[] values = new[] { "Germany", "Berlin", "Founder", "83120520", "357386", "3863344", "0.947", "96" };
        CsvRawRecord csvRawRecord = new CsvRawRecord(values, columnMap);

        // Act - call directly without reflection
        CsvCountryRecord record = reader.ParseLine(csvRawRecord);

        // Assert
        Assert.Equal("Germany", record.Name);
        Assert.Equal("Berlin", record.Capital);
        Assert.Equal("Founder", record.Accession);
        Assert.Equal(83120520, record.Population);
        Assert.Equal(357386d, record.Area);
    }


    [Fact]
    public void Constructor_WithValidPath_DoesNotThrow()
    {
        // Act & Assert
        Exception exception = Record.Exception(() => new CsvCountryFileReader("dummy.csv"));
        Assert.Null(exception);
    }

    [Fact]
    public void Constructor_WithNullPath_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CsvCountryFileReader(null!));
    }

    [Fact]
    public void Constructor_WithEmptyPath_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new CsvCountryFileReader(string.Empty));
    }

    [Fact]
    public void ReadAllRecords_FromTestData_LoadsCorrectly()
    {
        // Arrange
        string filePath = Path.Combine("TestData", "countries.csv");
        CsvCountryFileReader reader = new CsvCountryFileReader(filePath);

        // Act
        List<CsvCountryRecord> records = reader.ReadAllRecords().ToList();

        // Assert
        Assert.NotEmpty(records);

        // Check first record (Austria)
        CsvCountryRecord firstRecord = records.First();
        Assert.Equal("Austria", firstRecord.Name);
        Assert.Equal("Vienna", firstRecord.Capital);
        Assert.Equal(8926000, firstRecord.Population);
        Assert.Equal(83855, firstRecord.Area);
    }
}