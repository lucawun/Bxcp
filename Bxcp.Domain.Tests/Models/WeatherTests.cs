using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Xunit;

namespace Bxcp.Domain.Tests.Models;

public class WeatherTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesWeather()
    {
        // Arrange
        int day = 1;
        double maxTemp = 30.0;
        double minTemp = 20.0;

        // Act
        var weather = new Weather(day, maxTemp, minTemp);

        // Assert
        Assert.Equal(day, weather.Day);
        Assert.Equal(maxTemp, weather.MaxTemperature);
        Assert.Equal(minTemp, weather.MinTemperature);
    }

    [Fact]
    public void Constructor_WithZeroDay_ThrowsDomainException()
    {
        // Arrange
        int day = 0;
        double maxTemp = 30.0;
        double minTemp = 20.0;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Weather(day, maxTemp, minTemp));
        Assert.Equal("The day number must be a positive integer.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNegativeDay_ThrowsDomainException()
    {
        // Arrange
        int day = -1;
        double maxTemp = 30.0;
        double minTemp = 20.0;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Weather(day, maxTemp, minTemp));
        Assert.Equal("The day number must be a positive integer.", exception.Message);
    }

    [Fact]
    public void Constructor_WithMaxTempEqualToMinTemp_CreatesWeather()
    {
        // Arrange
        int day = 1;
        double maxTemp = 20.0;
        double minTemp = 20.0;

        // Act
        var weather = new Weather(day, maxTemp, minTemp);

        // Assert
        Assert.Equal(0.0, weather.TemperatureSpread);
    }

    [Fact]
    public void Constructor_WithMaxTempLessThanMinTemp_ThrowsDomainException()
    {
        // Arrange
        int day = 1;
        double maxTemp = 20.0;
        double minTemp = 30.0;

        // Act & Assert
        var exception = Assert.Throws<DomainException>(() => new Weather(day, maxTemp, minTemp));
        Assert.Equal("Max temperature cannot be less than min temperature.", exception.Message);
    }

    [Fact]
    public void TemperatureSpread_CalculatesCorrectValue()
    {
        // Arrange
        var weather = new Weather(1, 30.0, 20.0);
        double expectedSpread = 10.0;

        // Act
        double spread = weather.TemperatureSpread;

        // Assert
        Assert.Equal(expectedSpread, spread);
    }

    [Fact]
    public void TemperatureSpread_WithNegativeTemperatures_CalculatesCorrectly()
    {
        // Arrange
        var weather = new Weather(1, -10.0, -30.0);
        double expectedSpread = 20.0; 

        // Act
        double spread = weather.TemperatureSpread;

        // Assert
        Assert.Equal(expectedSpread, spread);
    }

    [Fact]
    public void TemperatureSpread_WithMixedSignTemperatures_CalculatesCorrectly()
    {
        // Arrange
        var weather = new Weather(1, 10.0, -5.0);
        double expectedSpread = 15.0; 

        // Act
        double spread = weather.TemperatureSpread;

        // Assert
        Assert.Equal(expectedSpread, spread);
    }
}