using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Xunit;

namespace Bxcp.Domain.Tests.Models;

public class WeatherTests
{
    [Fact]
    public void ConstructorWithValidDataCreatesWeather()
    {
        // Arrange
        const int day = 1;
        const double maxTemp = 30.0;
        const double minTemp = 20.0;

        // Act
        Weather weather = new(day, maxTemp, minTemp);

        // Assert
        Assert.Equal(day, weather.Day);
        Assert.Equal(maxTemp, weather.MaxTemperature);
        Assert.Equal(minTemp, weather.MinTemperature);
    }

    [Fact]
    public void ConstructorWithZeroDayThrowsDomainException()
    {
        // Arrange
        const int day = 0;
        const double maxTemp = 30.0;
        const double minTemp = 20.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Weather(day, maxTemp, minTemp));
        Assert.Equal("The day number must be a positive integer.", exception.Message);
    }

    [Fact]
    public void ConstructorWithNegativeDayThrowsDomainException()
    {
        // Arrange
        const int day = -1;
        const double maxTemp = 30.0;
        const double minTemp = 20.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Weather(day, maxTemp, minTemp));
        Assert.Equal("The day number must be a positive integer.", exception.Message);
    }

    [Fact]
    public void ConstructorWithMaxTempEqualToMinTempCreatesWeather()
    {
        // Arrange
        const int day = 1;
        const double maxTemp = 20.0;
        const double minTemp = 20.0;

        // Act
        Weather weather = new(day, maxTemp, minTemp);

        // Assert
        Assert.Equal(0.0, weather.TemperatureSpread);
    }

    [Fact]
    public void ConstructorWithMaxTempLessThanMinTempThrowsDomainException()
    {
        // Arrange
        const int day = 1;
        const double maxTemp = 20.0;
        const double minTemp = 30.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Weather(day, maxTemp, minTemp));
        Assert.Equal("Max temperature cannot be less than min temperature.", exception.Message);
    }

    [Fact]
    public void TemperatureSpreadCalculatesCorrectValue()
    {
        // Arrange
        Weather weather = new(1, 30.0, 20.0);
        const double expectedSpread = 10.0;

        // Act
        double spread = weather.TemperatureSpread;

        // Assert
        Assert.Equal(expectedSpread, spread);
    }

    [Fact]
    public void TemperatureSpreadWithNegativeTemperaturesCalculatesCorrectly()
    {
        // Arrange
        Weather weather = new(1, -10.0, -30.0);
        const double expectedSpread = 20.0;

        // Act
        double spread = weather.TemperatureSpread;

        // Assert
        Assert.Equal(expectedSpread, spread);
    }

    [Fact]
    public void TemperatureSpreadWithMixedSignTemperaturesCalculatesCorrectly()
    {
        // Arrange
        Weather weather = new(1, 10.0, -5.0);
        const double expectedSpread = 15.0;

        // Act
        double spread = weather.TemperatureSpread;

        // Assert
        Assert.Equal(expectedSpread, spread);
    }
}