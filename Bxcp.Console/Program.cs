using Microsoft.Extensions.DependencyInjection;

namespace Bxcp.Console;

public static class Program
{
    static void Main(string[] args)
    {
        string weatherFilePath = Path.Combine("TestData", "weather.csv");
        string countriesFilePath = Path.Combine("TestData", "countries.csv");

        ServiceProvider serviceProvider = new ServiceCollection()
            .ConfigureServices(weatherFilePath, countriesFilePath)
            .BuildServiceProvider();

        serviceProvider.RunWeatherAnalysis();
        serviceProvider.RunCountryAnalysis();

        System.Console.ReadKey();
    }
}