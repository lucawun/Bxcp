using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Xunit;

namespace Bxcp.Domain.Tests.Models;

public class CountryTests
{
    [Fact]
    public void ConstructorWithValidDataCreatesCountry()
    {
        // Arrange
        const string name = "TestCountry";
        const int population = 1000000;
        const double area = 1000.0;

        // Act
        Country country = new(name, population, area);

        // Assert
        Assert.Equal(name, country.Name);
        Assert.Equal(population, country.Population);
        Assert.Equal(area, country.Area);
    }

    [Fact]
    public void ConstructorWithEmptyNameThrowsDomainException()
    {
        // Arrange
        const string name = "";
        const int population = 1000000;
        const double area = 1000.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Country name cannot be empty.", exception.Message);
    }

    [Fact]
    public void ConstructorWithNullNameThrowsDomainException()
    {
        // Arrange
        const string? name = null;
        const int population = 1000000;
        const double area = 1000.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name!, population, area));
        Assert.Equal("Country name cannot be empty.", exception.Message);
    }

    [Fact]
    public void ConstructorWithNegativePopulationThrowsDomainException()
    {
        // Arrange
        const string name = "TestCountry";
        const int population = -1000;
        const double area = 1000.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Population cannot be negative.", exception.Message);
    }

    [Fact]
    public void ConstructorWithZeroAreaThrowsDomainException()
    {
        // Arrange
        const string name = "TestCountry";
        const int population = 1000000;
        const double area = 0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Area must be greater than zero.", exception.Message);
    }

    [Fact]
    public void ConstructorWithNegativeAreaThrowsDomainException()
    {
        // Arrange
        const string name = "TestCountry";
        const int population = 1000000;
        const double area = -100;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Area must be greater than zero.", exception.Message);
    }

    [Fact]
    public void PopulationDensityCalculatesCorrectValue()
    {
        // Arrange
        Country country = new("TestCountry", 10000, 100);
        const double expectedDensity = 100.0;

        // Act
        double density = country.PopulationDensity;

        // Assert
        Assert.Equal(expectedDensity, density);
    }

    [Fact]
    public void PopulationDensityWithLargePopulationCalculatesCorrectly()
    {
        // Arrange
        Country country = new("TestCountry", 1000000000, 1000);
        const double expectedDensity = 1000000.0;

        // Act
        double density = country.PopulationDensity;

        // Assert
        Assert.Equal(expectedDensity, density);
    }
}