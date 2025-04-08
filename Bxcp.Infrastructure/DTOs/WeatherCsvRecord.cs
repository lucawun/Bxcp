namespace Bxcp.Infrastructure.DTOs;

/// <summary>
/// Data Transfer Object representing a single row from the weather CSV file
/// </summary>
public record WeatherCsvRecord
{
    public int Day { get; init; }
    public double MxT { get; init; }
    public double MnT { get; init; }
    public double AvT { get; init; }
    public double AvDP { get; init; }
    public double TPcpn { get; init; }
    public int PDir { get; init; }
    public double AvSp { get; init; }
    public int Dir { get; init; }
    public int MxS { get; init; }
    public double SkyC { get; init; }
    public int MxR { get; init; }
    public int Mn { get; init; }
    public double R { get; init; }
    public double AvSLP { get; init; }
}