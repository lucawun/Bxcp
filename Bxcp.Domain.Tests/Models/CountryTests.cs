using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Xunit;

namespace Bxcp.Domain.Tests.Models;

public class CountryTests
{
    [Fact]
    public void Constructor_WithValidData_CreatesCountry()
    {
        // Arrange
        string name = "TestCountry";
        int population = 1000000;
        double area = 1000.0;

        // Act
        Country country = new Country(name, population, area);

        // Assert
        Assert.Equal(name, country.Name);
        Assert.Equal(population, country.Population);
        Assert.Equal(area, country.Area);
    }

    [Fact]
    public void Constructor_WithEmptyName_ThrowsDomainException()
    {
        // Arrange
        string name = "";
        int population = 1000000;
        double area = 1000.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Country name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullName_ThrowsDomainException()
    {
        // Arrange
        string? name = null;
        int population = 1000000;
        double area = 1000.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name!, population, area));
        Assert.Equal("Country name cannot be empty.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNegativePopulation_ThrowsDomainException()
    {
        // Arrange
        string name = "TestCountry";
        int population = -1000;
        double area = 1000.0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Population cannot be negative.", exception.Message);
    }

    [Fact]
    public void Constructor_WithZeroArea_ThrowsDomainException()
    {
        // Arrange
        string name = "TestCountry";
        int population = 1000000;
        double area = 0;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Area must be greater than zero.", exception.Message);
    }

    [Fact]
    public void Constructor_WithNegativeArea_ThrowsDomainException()
    {
        // Arrange
        string name = "TestCountry";
        int population = 1000000;
        double area = -100;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => new Country(name, population, area));
        Assert.Equal("Area must be greater than zero.", exception.Message);
    }

    [Fact]
    public void PopulationDensity_CalculatesCorrectValue()
    {
        // Arrange
        Country country = new Country("TestCountry", 10000, 100);
        double expectedDensity = 100.0; 

        // Act
        double density = country.PopulationDensity;

        // Assert
        Assert.Equal(expectedDensity, density);
    }

    [Fact]
    public void PopulationDensity_WithLargePopulation_CalculatesCorrectly()
    {
        // Arrange
        Country country = new Country("TestCountry", 1000000000, 1000);
        double expectedDensity = 1000000.0;

        // Act
        double density = country.PopulationDensity;

        // Assert
        Assert.Equal(expectedDensity, density);
    }
}