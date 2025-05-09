﻿using Bxcp.Domain.DomainServices;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;
using Xunit;

namespace Bxcp.Domain.Tests.DomainServices;

public class CountryStatisticsServiceTests
{
    private readonly CountryStatisticsService _service;

    public CountryStatisticsServiceTests()
    {
        _service = new CountryStatisticsService();
    }

    [Fact]
    public void FindHighestPopulationDensityWithValidDataReturnsCorrectCountry()
    {
        // Arrange
        List<Country> countries =
        [
            new Country("Country1", 1000000, 1000), // density = 1000
            new Country("Country2", 500000, 100),   // density = 5000 (highest)
            new Country("Country3", 2000000, 5000)  // density = 400
        ];

        // Act
        Country result = _service.FindHighestPopulationDensity(countries);

        // Assert
        Assert.Equal("Country2", result.Name);
        Assert.Equal(5000, result.PopulationDensity);
    }

    [Fact]
    public void FindHighestPopulationDensityWithEmptyListThrowsDomainException()
    {
        // Arrange
        List<Country> countries = [];

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => _service.FindHighestPopulationDensity(countries));
        Assert.Equal("Country records cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void FindHighestPopulationDensityWithNullListThrowsDomainException()
    {
        // Arrange
        List<Country>? countries = null;

        // Act & Assert
        DomainException exception = Assert.Throws<DomainException>(() => _service.FindHighestPopulationDensity(countries!));
        Assert.Equal("Country records cannot be null or empty.", exception.Message);
    }

    [Fact]
    public void FindHighestPopulationDensityWithMultipleSameValuesReturnsFirst()
    {
        // Arrange
        List<Country> countries =
        [
            new Country("Country1", 10000, 10), // density = 1000
            new Country("Country2", 20000, 20), // density = 1000 (same)
            new Country("Country3", 5000, 10)   // density = 500
        ];

        // Act
        Country result = _service.FindHighestPopulationDensity(countries);

        // Assert
        Assert.Equal("Country1", result.Name); // Should return the first one with highest density
    }

    [Fact]
    public void FindHighestPopulationDensityWithExtremeValuesReturnsCorrectResult()
    {
        // Arrange
        List<Country> countries =
        [
            new Country("Tiny", 100, 0.01),          // density = 10000 (highest)
            new Country("Medium", 1000000, 1000),    // density = 1000
            new Country("Huge", 1000000000, 10000000) // density = 100
        ];

        // Act
        Country result = _service.FindHighestPopulationDensity(countries);

        // Assert
        Assert.Equal("Tiny", result.Name);
        Assert.Equal(10000, result.PopulationDensity);
    }

    [Fact]
    public void FindHighestPopulationDensityWithSingleCountryReturnsThatCountry()
    {
        // Arrange
        List<Country> countries =
        [
            new Country("OnlyOne", 1000000, 1000) // density = 1000
        ];

        // Act
        Country result = _service.FindHighestPopulationDensity(countries);

        // Assert
        Assert.Equal("OnlyOne", result.Name);
        Assert.Equal(1000, result.PopulationDensity);
    }
}