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
}