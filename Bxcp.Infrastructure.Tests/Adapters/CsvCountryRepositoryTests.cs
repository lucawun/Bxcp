using Bxcp.Infrastructure.Adapters;
using Bxcp.Infrastructure.DataAccess.CsvHelper;
using Moq;
using Xunit;

namespace Bxcp.Infrastructure.Tests.Adapters;

public class CsvCountryRepositoryTests
{
    [Fact]
    public void ConstructorNullFileReaderThrowsArgumentNullException() =>
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CsvCountryRepository(null!));

    [Fact]
    public void ReadAllRecordsReturnsCorrectlyMappedDomainEntities()
    {
        // Arrange
        Mock<CsvCountryFileReader> mockFileReader = new("dummy/path");
        mockFileReader.Setup(fr => fr.ReadAllRecords()).Returns(
            [
                new() {
                    Name = "Germany",
                    Population = 83000000,
                    Area = 357022,
                    Capital = "Berlin",
                    Accession = "1957",
                    GDP = "3806",
                    HDI = "0.947",
                    MEPs = "96"
                },
                new() {
                    Name = "France",
                    Population = 67000000,
                    Area = 643801,
                    Capital = "Paris",
                    Accession = "1957",
                    GDP = "2716",
                    HDI = "0.901",
                    MEPs = "79"
                }
            ]);

        CsvCountryRepository repository = new(mockFileReader.Object);

        // Act
        List<Domain.Models.Country> result = [.. repository.ReadAllRecords()];

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
    public void ReadAllRecordsEmptySourceReturnsEmptyCollection()
    {
        // Arrange
        Mock<CsvCountryFileReader> mockFileReader = new("dummy/path");
        mockFileReader.Setup(fr => fr.ReadAllRecords()).Returns([]);

        CsvCountryRepository repository = new(mockFileReader.Object);

        // Act
        IEnumerable<Domain.Models.Country> result = repository.ReadAllRecords();

        // Assert
        Assert.Empty(result);
    }
}