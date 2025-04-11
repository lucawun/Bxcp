using Moq;
using Xunit;

using Bxcp.Infrastructure.Adapters;
using Bxcp.Infrastructure.DataAccess.CsvHelper;
using Bxcp.Infrastructure.DTOs;

namespace Bxcp.Infrastructure.Tests.Adapters;

public class CsvWeatherRepositoryTests
{
    [Fact]
    public void Constructor_NullFileReader_ThrowsArgumentNullException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CsvWeatherRepository(null!));
    }

    [Fact]
    public void ReadAllRecords_ReturnsCorrectlyMappedDomainEntities()
    {
        // Arrange
        var mockFileReader = new Mock<CsvWeatherFileReader>("dummy/path");
        mockFileReader.Setup(fr => fr.ReadAllRecords()).Returns(new List<CsvWeatherRecord>
            {
                new CsvWeatherRecord
                {
                    Day = 1,
                    MxT = 30.5,
                    MnT = 15.2,
                    AvT = 22.8,
                    AvDP = 14.5,
                    TPcpn = 0,
                    PDir = 180,
                    AvSp = 5.3,
                    Dir = 220,
                    MxS = 10,
                    SkyC = 2.0,
                    MxR = 0,
                    Mn = 0,
                    R = 0.0,
                    AvSLP = 1012.2
                },
                new CsvWeatherRecord
                {
                    Day = 2,
                    MxT = 28.3,
                    MnT = 17.5,
                    AvT = 23.4,
                    AvDP = 15.1,
                    TPcpn = 0.2,
                    PDir = 200,
                    AvSp = 4.8,
                    Dir = 240,
                    MxS = 8,
                    SkyC = 1.5,
                    MxR = 1,
                    Mn = 0,
                    R = 0.5,
                    AvSLP = 1010.8
                }
            });

        var repository = new CsvWeatherRepository(mockFileReader.Object);

        // Act
        var result = repository.ReadAllRecords().ToList();

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(1, result[0].Day);
        Assert.Equal(30.5, result[0].MaxTemperature);
        Assert.Equal(15.2, result[0].MinTemperature);

        Assert.Equal(2, result[1].Day);
        Assert.Equal(28.3, result[1].MaxTemperature);
        Assert.Equal(17.5, result[1].MinTemperature);
    }

    [Fact]
    public void ReadAllRecords_EmptySource_ReturnsEmptyCollection()
    {
        // Arrange
        var mockFileReader = new Mock<CsvWeatherFileReader>("dummy/path");
        mockFileReader.Setup(fr => fr.ReadAllRecords()).Returns(new List<CsvWeatherRecord>());

        var repository = new CsvWeatherRepository(mockFileReader.Object);

        // Act
        var result = repository.ReadAllRecords();

        // Assert
        Assert.Empty(result);
    }
}