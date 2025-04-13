using Bxcp.Application.DTOs;
using Bxcp.Application.Exceptions;
using Bxcp.Application.UseCases;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;
using Moq;
using Xunit;

namespace Bxcp.Application.Tests.UseCases;

public class ClimateAnalysisUseCaseTests
{
    private readonly Mock<IDataProviderRepository<Weather>> _mockRepository;
    private readonly Mock<IClimateService> _mockClimateService;
    private readonly ClimateAnalysisUseCase _useCase;

    public ClimateAnalysisUseCaseTests()
    {
        _mockRepository = new Mock<IDataProviderRepository<Weather>>();
        _mockClimateService = new Mock<IClimateService>();
        _useCase = new ClimateAnalysisUseCase(_mockRepository.Object, _mockClimateService.Object);
    }

    [Fact]
    public void ConstructorNullRepositoryThrowsArgumentNullException() =>
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ClimateAnalysisUseCase(null!, _mockClimateService.Object));

    [Fact]
    public void ConstructorNullClimateServiceThrowsArgumentNullException() =>
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ClimateAnalysisUseCase(_mockRepository.Object, null!));

    [Fact]
    public void AnalyzeClimateHappyPathReturnsCorrectResult()
    {
        // Arrange
        List<Weather> weatherRecords =
        [
            new Weather(1, 30.0, 20.0),
            new Weather(2, 25.0, 15.0),
            new Weather(3, 35.0, 25.0)
        ];

        Weather smallestSpreadWeather = new(2, 25.0, 15.0);

        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(weatherRecords);
        _mockClimateService.Setup(s => s.FindSmallestTemperatureSpread(weatherRecords))
            .Returns(smallestSpreadWeather);

        // Act
        ClimateAnalysisResult result = _useCase.AnalyzeClimate();

        // Assert
        Assert.Equal(2, result.DayWithSmallestTemperatureSpread);
        Assert.Equal(10.0, result.SmallestTemperatureSpread);

        _mockRepository.Verify(r => r.ReadAllRecords(), Times.Once);
        _mockClimateService.Verify(s => s.FindSmallestTemperatureSpread(weatherRecords), Times.Once);
    }

    [Fact]
    public void AnalyzeClimateRepositoryThrowsExceptionThrowsAnalysisFailedException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ReadAllRecords()).Throws(new InvalidOperationException("Repository error"));

        // Act & Assert
        AnalysisFailedException exception = Assert.Throws<AnalysisFailedException>(() => _useCase.AnalyzeClimate());
        Assert.Contains("Failed to analyze Climate data", exception.Message, StringComparison.Ordinal);
        Assert.Contains("Repository error", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void AnalyzeClimateServiceThrowsExceptionThrowsAnalysisFailedException()
    {
        // Arrange
        List<Weather> weatherRecords = [new Weather(1, 30.0, 20.0)];
        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(weatherRecords);
        _mockClimateService.Setup(s => s.FindSmallestTemperatureSpread(weatherRecords))
            .Throws(new DomainException("Service error"));

        // Act & Assert
        AnalysisFailedException exception = Assert.Throws<AnalysisFailedException>(() => _useCase.AnalyzeClimate());
        Assert.Contains("Failed to analyze Climate data", exception.Message, StringComparison.Ordinal);
        Assert.Contains("Service error", exception.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void AnalyzeClimateServiceReturnsExpectedDataMapsCorrectly()
    {
        // Arrange
        List<Weather> weatherRecords =
        [
            new Weather(1, 30.0, 20.0)
        ];

        Weather resultWeather = new(1, 30.0, 20.0); // Spread = 10

        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(weatherRecords);
        _mockClimateService.Setup(s => s.FindSmallestTemperatureSpread(weatherRecords))
            .Returns(resultWeather);

        // Act
        ClimateAnalysisResult result = _useCase.AnalyzeClimate();

        // Assert
        Assert.Equal(1, result.DayWithSmallestTemperatureSpread);
        Assert.Equal(10.0, result.SmallestTemperatureSpread);
    }
}