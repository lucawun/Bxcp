using Moq;
using Xunit;

using Bxcp.Infrastructure.Adapters;
using Bxcp.Infrastructure.DataAccess.CsvHelper;
using Bxcp.Infrastructure.DTOs;

namespace Bxcp.Infrastructure.Tests.Adapters;

public class CsvCountryRepositoryTests
{
    [Fact]
    public void Constructor_NullFileReader_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CsvCountryRepository(null!));
    }

    [Fact]
    public void ReadAllRecords_ReturnsCorrectlyMappedDomainEntities()
    {
        // Arrange
        var mockFileReader = new Mock<CsvCountryFileReader>("dummy/path");
        mockFileReader.Setup(fr => fr.ReadAllRecords()).Returns(new List<CsvCountryRecord>
            {
                new CsvCountryRecord
                {
                    Name = "Germany",
                    Population = 83000000,
                    Area = 357022,
                    Capital = "Berlin",
                    Accession = "1957",
                    GDP = "3806",
                    HDI = "0.947",
                    MEPs = "96"
                },
                new CsvCountryRecord
                {
                    Name = "France",
                    Population = 67000000,
                    Area = 643801,
                    Capital = "Paris",
                    Accession = "1957",
                    GDP = "2716",
                    HDI = "0.901",
                    MEPs = "79"
                }
            });

        var repository = new CsvCountryRepository(mockFileReader.Object);

        // Act
        var result = repository.ReadAllRecords().ToList();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal("Germany", result[0].Name);
        Assert.Equal(83000000, result[0].Population);
        Assert.Equal(357022, result[0].Area);

        Assert.Equal("France", result[1].Name);
        Assert.Equal(67000000, result[1].Population);
        Assert.Equal(643801, result[1].Area);
    }

    [Fact]
    public void ReadAllRecords_EmptySource_ReturnsEmptyCollection()
    {
        // Arrange
        var mockFileReader = new Mock<CsvCountryFileReader>("dummy/path");
        mockFileReader.Setup(fr => fr.ReadAllRecords()).Returns(new List<CsvCountryRecord>());

        var repository = new CsvCountryRepository(mockFileReader.Object);

        // Act
        var result = repository.ReadAllRecords();

        // Assert
        Assert.Empty(result);
    }
}