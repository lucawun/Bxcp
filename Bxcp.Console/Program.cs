using Bxcp.Application.DTOs;
using Bxcp.Application.Ports.Driving;
using Microsoft.Extensions.DependencyInjection;

namespace Bxcp.Console;

public static class Program
{
    static void Main(string[] args)
    {
        string weatherFilePath = Path.Combine("Resources", "weather.csv");
        string countriesFilePath = Path.Combine("Resources", "countries.csv");

        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(weatherFilePath, countriesFilePath)
            .BuildServiceProvider();

        serviceProvider.RunWeatherAnalysis();
        serviceProvider.RunCountryAnalysis();

        System.Console.ReadKey();
    }

    public static void RunWeatherAnalysis(this ServiceProvider serviceProvider)
    {
        try
        {
            IClimateAnalysisUseCase weatherAnalysisPort = serviceProvider.GetRequiredService<IClimateAnalysisUseCase>();
            ClimateAnalysisResult result = weatherAnalysisPort.AnalyzeClimate();

            System.Console.WriteLine("===== Weather Analysis =====");
            System.Console.WriteLine($"Day with smallest temperature range: Day {result.DayWithSmallestTemperatureSpread}");
            System.Console.WriteLine($"Smallest temperature range: {result.SmallestTemperatureSpread:F2}°C");
            System.Console.WriteLine();
        }
        catch (InvalidOperationException ex)
        {
            System.Console.WriteLine($"Error during weather analysis: {ex.Message}");
        }
    }

    public static void RunCountryAnalysis(this ServiceProvider serviceProvider)
    {
        try
        {
            ICountryAnalysisStatisticsUseCase countryAnalysisPort = serviceProvider.GetRequiredService<ICountryAnalysisStatisticsUseCase>();
            CountryAnalysisResult result = countryAnalysisPort.AnalyzeCountryStatistics();

            System.Console.WriteLine("===== Country Analysis =====");
            System.Console.WriteLine($"Country with highest population density: {result.CountryWithHighestDensity}");
            System.Console.WriteLine($"Highest population density: {result.HighestDensity:F2} inhabitants per km²");
            System.Console.WriteLine();
        }
        catch (InvalidOperationException ex)
        {
            System.Console.WriteLine($"Error during country analysis: {ex.Message}");
        }
    }
}