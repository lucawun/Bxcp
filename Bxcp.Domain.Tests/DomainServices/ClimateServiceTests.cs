using Bxcp.Domain.DomainServices;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Xunit;

namespace Bxcp.Domain.Tests.DomainServices;

public class ClimateServiceTests
{
    private readonly ClimateService _service;

    public ClimateServiceTests()
    {
        _service = new ClimateService();
    }

    [Fact]
    public void FindSmallestTemperatureSpread_WithValidData_ReturnsCorrectRecord()
    {
        // Arrange
        List<Weather> records = new List<Weather>
        {
            new Weather(1, 30.0, 20.0), // spread = 10
            new Weather(2, 25.0, 20.0), // spread = 5 (smallest)
            new Weather(3, 35.0, 15.0)  // spread = 20
        };

        // Act
        Weather result = _service.FindSmallestTemperatureSpread(records);

        // Assert
        Assert.Equal(2, result.Day);
        Assert.Equal(5.0, result.TemperatureSpread);
    }

    [Fact]
    public void FindSmallestTemperatureSpread_WithEmptyList_ThrowsDomainException()
    {
        // Arrange
        List<Weather> records = new List<Weather>();

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => _service.FindSmallestTemperatureSpread(records));
        Assert.Equal("Weather records cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void FindSmallestTemperatureSpread_WithNullList_ThrowsDomainException()
    {
        // Arrange
        List<Weather>? records = null;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => _service.FindSmallestTemperatureSpread(records!));
        Assert.Equal("Weather records cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void FindSmallestTemperatureSpread_WithMultipleSameValues_ReturnsFirst()
    {
        // Arrange
        List<Weather> records = new List<Weather>
        {
            new Weather(1, 30.0, 20.0), // spread = 10
            new Weather(2, 25.0, 15.0), // spread = 10 (same as first)
            new Weather(3, 35.0, 25.0)  // spread = 10 (same as first)
        };

        // Act
        Weather result = _service.FindSmallestTemperatureSpread(records);

        // Assert
        Assert.Equal(1, result.Day); // Should return the first one with the smallest spread
    }

    [Fact]
    public void FindSmallestTemperatureSpread_WithZeroSpread_ReturnsCorrectRecord()
    {
        // Arrange
        List<Weather> records = new List<Weather>
        {
            new Weather(1, 30.0, 20.0), // spread = 10
            new Weather(2, 25.0, 25.0), // spread = 0 (smallest)
            new Weather(3, 35.0, 15.0)  // spread = 20
        };

        // Act
        Weather result = _service.FindSmallestTemperatureSpread(records);

        // Assert
        Assert.Equal(2, result.Day);
        Assert.Equal(0.0, result.TemperatureSpread);
    }

    [Fact]
    public void FindSmallestTemperatureSpread_WithNegativeTemperatures_WorksCorrectly()
    {
        // Arrange
        List<Weather> records = new List<Weather>
        {
            new Weather(1, -5.0, -10.0),   // spread = 5
            new Weather(2, -15.0, -20.0),  // spread = 5 (same)
            new Weather(3, 0.0, -30.0)     // spread = 30
        };

        // Act
        Weather result = _service.FindSmallestTemperatureSpread(records);

        // Assert
        Assert.Equal(1, result.Day); // Should return the first record with spread = 5
        Assert.Equal(5.0, result.TemperatureSpread);
    }
}