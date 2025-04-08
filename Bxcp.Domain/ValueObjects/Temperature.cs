namespace Bxcp.Domain.ValueObjects;

/// <summary>
/// Represents a temperature value with conversion capabilities.
/// </summary>
public record Temperature
{
    private const double AbsoluteZeroInCelsius = -273.15;
    private const double FahrenheitFreezingPoint = 32;
    private const double FahrenheitToCelsiusRatio = 5.0 / 9.0;
    private const double CelsiusToFahrenheitRatio = 9.0 / 5.0;

    public double Celsius { get; }
    private Temperature(double celsius)
    {
        Celsius = celsius;
    }

    public static Temperature FromCelsius(double celsius) => new(celsius);

    public static Temperature FromFahrenheit(double fahrenheit) =>
        new((fahrenheit - FahrenheitFreezingPoint) * FahrenheitToCelsiusRatio);

    public static Temperature FromKelvin(double kelvin) =>
        new(kelvin + AbsoluteZeroInCelsius);

    public double ToFahrenheit() =>
        Celsius * CelsiusToFahrenheitRatio + FahrenheitFreezingPoint;

    public double ToKelvin() =>
        Celsius - AbsoluteZeroInCelsius;

    public static implicit operator double(Temperature temperature) => temperature.Celsius;

    public override string ToString() => $"{Celsius:F1}°C";
}