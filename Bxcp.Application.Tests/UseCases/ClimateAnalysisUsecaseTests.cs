using Bxcp.Application.DTOs;
using Bxcp.Application.Exceptions;
using Bxcp.Application.UseCases;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports.Incoming;
using Bxcp.Domain.Repositories;
using Moq;
using Xunit;

namespace Bxcp.Application.Tests.UseCases;

public class ClimateAnalysisUsecaseTests
{
    private readonly Mock<IRepository<Weather>> _mockRepository;
    private readonly Mock<IClimateService> _mockClimateService;
    private readonly ClimateAnalysisUsecase _useCase;

    public ClimateAnalysisUsecaseTests()
    {
        _mockRepository = new Mock<IRepository<Weather>>();
        _mockClimateService = new Mock<IClimateService>();
        _useCase = new ClimateAnalysisUsecase(_mockRepository.Object, _mockClimateService.Object);
    }

    [Fact]
    public void Constructor_NullRepository_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ClimateAnalysisUsecase(null!, _mockClimateService.Object));
    }

    [Fact]
    public void Constructor_NullClimateService_ThrowsArgumentNullException()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ClimateAnalysisUsecase(_mockRepository.Object, null!));
    }

    [Fact]
    public void AnalyzeClimate_HappyPath_ReturnsCorrectResult()
    {
        // Arrange
        var weatherRecords = new List<Weather>
        {
            new Weather(1, 30.0, 20.0),
            new Weather(2, 25.0, 15.0),
            new Weather(3, 35.0, 25.0)
        };

        var smallestSpreadWeather = new Weather(2, 25.0, 15.0);

        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(weatherRecords);
        _mockClimateService.Setup(s => s.FindSmallestTemperatureSpread(weatherRecords))
            .Returns(smallestSpreadWeather);

        // Act
        var result = _useCase.AnalyzeClimate();

        // Assert
        Assert.Equal(2, result.DayWithSmallestTemperatureSpread);
        Assert.Equal(10.0, result.SmallestTemperatureSpread);

        _mockRepository.Verify(r => r.ReadAllRecords(), Times.Once);
        _mockClimateService.Verify(s => s.FindSmallestTemperatureSpread(weatherRecords), Times.Once);
    }

    [Fact]
    public void AnalyzeClimate_RepositoryThrowsException_ThrowsAnalysisFailedException()
    {
        // Arrange
        _mockRepository.Setup(r => r.ReadAllRecords()).Throws(new Exception("Repository error"));

        // Act & Assert
        var exception = Assert.Throws<AnalysisFailedException>(() => _useCase.AnalyzeClimate());
        Assert.Contains("Failed to analyze Climate data", exception.Message);
        Assert.Contains("Repository error", exception.Message);
    }

    [Fact]
    public void AnalyzeClimate_ServiceThrowsException_ThrowsAnalysisFailedException()
    {
        // Arrange
        var weatherRecords = new List<Weather> { new Weather(1, 30.0, 20.0) };
        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(weatherRecords);
        _mockClimateService.Setup(s => s.FindSmallestTemperatureSpread(weatherRecords))
            .Throws(new DomainException("Service error"));

        // Act & Assert
        var exception = Assert.Throws<AnalysisFailedException>(() => _useCase.AnalyzeClimate());
        Assert.Contains("Failed to analyze Climate data", exception.Message);
        Assert.Contains("Service error", exception.Message);
    }

    [Fact]
    public void AnalyzeClimate_ServiceReturnsExpectedData_MapsCorrectly()
    {
        // Arrange
        var weatherRecords = new List<Weather>
        {
            new Weather(1, 30.0, 20.0)
        };

        var resultWeather = new Weather(1, 30.0, 20.0); // Spread = 10

        _mockRepository.Setup(r => r.ReadAllRecords()).Returns(weatherRecords);
        _mockClimateService.Setup(s => s.FindSmallestTemperatureSpread(weatherRecords))
            .Returns(resultWeather);

        // Act
        var result = _useCase.AnalyzeClimate();

        // Assert
        Assert.Equal(1, result.DayWithSmallestTemperatureSpread);
        Assert.Equal(10.0, result.SmallestTemperatureSpread);
    }
}