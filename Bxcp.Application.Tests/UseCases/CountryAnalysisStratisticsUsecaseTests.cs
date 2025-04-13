using Bxcp.Application.UseCases;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Moq;
using Xunit;

namespace Bxcp.Application.Tests.UseCases;

public class CountryAnalysisStratisticsUseCaseTests
{
    private readonly Mock<IDataProviderRepository<Country>> _mockRepository;
    private readonly Mock<ICountryStatisticsService> _mockCountryService;
    private readonly CountryAnalysisStratisticsUseCase _useCase;

    public CountryAnalysisStratisticsUseCaseTests()
    {
        _mockRepository = new Mock<IDataProviderRepository<Country>>();
        _mockCountryService = new Mock<ICountryStatisticsService>();
        _useCase = new CountryAnalysisStratisticsUseCase(_mockRepository.Object, _mockCountryService.Object);
    }

    [Fact]
    public void ConstructorNullRepositoryThrowsArgumentNullException() =>
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CountryAnalysisStratisticsUseCase(null!, _mockCountryService.Object));

    [Fact]
    public void ConstructorNullCountryServiceThrowsArgumentNullException() =>
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new CountryAnalysisStratisticsUseCase(_mockRepository.Object, null!));

    [Fact]
    public void AnalyzeCountryStatisticsHappyPathReturnsCorrectResult()
    {
        // Arrange
        List<Country> countries =
        [
            new Country("Country1", 1000000, 1000),
            new Country("Country2", 500000, 100),
            new Country("Country3", 2000000, 5000)
        ];

        Country highestDensityCountry = new("Country2", 500000, 100); // density = 5000

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
    public void AnalyzeCountryStatisticsServiceReturnsExpectedDataMapsCorrectly()
    {
        // Arrange
        List<Country> countries =
        [
            new Country("MicroCountry", 100000, 10) // Density = 10000
        ];

        Country resultCountry = new("MicroCountry", 100000, 10);

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