using Bxcp.Infrastructure.DTOs.FileSystem;

namespace Bxcp.Infrastructure.Adapters.FileSystem;

/// <summary>
/// Reads weather data from CSV files
/// </summary>
public class CsvWeatherFileReader : BaseCsvFileReader<CsvWeatherRecord>
{
    public CsvWeatherFileReader(string filePath) : base(filePath, ',')
    {
    }

    /// <inheritdoc />
    internal override string[] GetRequiredColumns()
    {
        return new[] { "Day", "MxT", "MnT" };
    }

    /// <inheritdoc />
    internal override CsvWeatherRecord ParseLine(CsvRawRecord rowRecord)
    {
        return new CsvWeatherRecord
        {
            Day = rowRecord.GetInt("Day"),
            MxT = rowRecord.GetDouble("MxT"),
            MnT = rowRecord.GetDouble("MnT"),
            AvT = rowRecord.GetDouble("AvT"),
            AvDP = rowRecord.GetDouble("AvDP"),
            TPcpn = rowRecord.GetDouble("1HrP TPcpn"),
            PDir = rowRecord.GetInt("PDir"),
            AvSp = rowRecord.GetDouble("AvSp"),
            Dir = rowRecord.GetInt("Dir"),
            MxS = rowRecord.GetInt("MxS"),
            SkyC = rowRecord.GetDouble("SkyC"),
            MxR = rowRecord.GetInt("MxR"),
            Mn = rowRecord.GetInt("Mn"),
            R = rowRecord.GetDouble("R"),
            AvSLP = rowRecord.GetDouble("AvSLP")
        };
    }
}