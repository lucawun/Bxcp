namespace Bxcp.Domain.ValueObjects;

/// <summary>
/// Represents a temperature value with conversion capabilities.
/// </summary>
public record Temperature
{
    public double Celsius { get; }
    private Temperature(double celsius)
    {
        Celsius = celsius;
    }
        public static Temperature FromCelsius(double celsius) => new(celsius);

    public static Temperature FromFahrenheit(double fahrenheit) => new((fahrenheit - 32) * 5 / 9);

    public double ToFahrenheit() => Celsius * 9 / 5 + 32;

    public static implicit operator double(Temperature temperature) => temperature.Celsius;

    public override string ToString() => $"{Celsius:F1}°C";
}