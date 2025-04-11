using Bxcp.Application.Exceptions;
using Bxcp.Application.UseCases;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Moq;
using Xunit;

namespace Bxcp.Application.Tests.UseCases;

public class CountryAnalysisStratisticsUsecaseTests
{
    private readonly Mock<IDataProviderRepository<Country>> _mockRepository;
    private readonly Mock<ICountryStatisticsService> _mockCountryService;
    private readonly CountryAnalysisStratisticsUsecase _useCase;

    public CountryAnalysisStratisticsUsecaseTests()
    {
        _mockRepository = new Mock<IDataProviderRepository<Country>>();
        _mockCountryService = new Mock<ICountryStatisticsService>();
        _useCase = new CountryAnalysisStratisticsUsecase(_mockRepository.Object, _mockCountryService.Object);
    }

    [Fact]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CountryAnalysisStratisticsUsecase(null!, _mockCountryService.Object));
    }

    [Fact]
    public void Constructor_NullCountryService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CountryAnalysisStratisticsUsecase(_mockRepository.Object, null!));
    }

    [Fact]
    public void AnalyzeCountryStatistics_HappyPath_ReturnsCorrectResult()
    {
        // Arrange
        List<Country> countries = new List<Country>
        {
            new Country("Country1", 1000000, 1000),
            new Country("Country2", 500000, 100),
            new Country("Country3", 2000000, 5000)
        };

        Country highestDensityCountry = new Country("Country2", 500000, 100); // density = 5000

        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(countries);
        _mockCountryService.Setup(s => s.FindHighestPopulationDensity(countries))
            .Returns(highestDensityCountry);

        // Act
        DTOs.CountryAnalysisResult result = _useCase.AnalyzeCountryStatistics();

        // Assert
        Assert.Equal("Country2", result.CountryWithHighestDensity);
        Assert.Equal(5000, result.HighestDensity);

        _mockRepository.Verify(r => r.ReadAllRecords(), Times.Once);
        _mockCountryService.Verify(s => s.FindHighestPopulationDensity(countries), Times.Once);
    }


    [Fact]
    public void AnalyzeCountryStatistics_ServiceReturnsExpectedData_MapsCorrectly()
    {
        // Arrange
        List<Country> countries = new List<Country>
        {
            new Country("MicroCountry", 100000, 10) // Density = 10000
        };

        Country resultCountry = new Country("MicroCountry", 100000, 10);

        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(countries);
        _mockCountryService.Setup(s => s.FindHighestPopulationDensity(countries))
            .Returns(resultCountry);

        // Act
        DTOs.CountryAnalysisResult result = _useCase.AnalyzeCountryStatistics();

        // Assert
        Assert.Equal("MicroCountry", result.CountryWithHighestDensity);
        Assert.Equal(10000, result.HighestDensity);
    }
}