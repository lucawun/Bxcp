using CsvHelper;
using CsvHelper.Configuration;

using Bxcp.Infrastructure.DTOs;
using Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper;

/// <summary>
/// Reads weather data from CSV files
/// </summary>
public class CsvWeatherFileReader : CsvBaseFileReader<CsvWeatherRecord>
{
    public CsvWeatherFileReader(string filePath) : base(filePath, ',')
    {
    }

    /// <inheritdoc />
    protected override void RegisterMapping(CsvReader csvReader)
    {
        csvReader.Context.RegisterClassMap<WeatherRecordMap>();
    }

    /// <summary>
    /// Maps CSV columns to CsvWeatherRecord DTO
    /// </summary>
    private class WeatherRecordMap : ClassMap<CsvWeatherRecord>
    {
        public WeatherRecordMap()
        {
            Map(m => m.Day).Name("Day");
            Map(m => m.MxT).Name("MxT");
            Map(m => m.MnT).Name("MnT");
            Map(m => m.AvT).Name("AvT");
            Map(m => m.AvDP).Name("AvDP");
            Map(m => m.TPcpn).Name("1HrP TPcpn");
            Map(m => m.PDir).Name("PDir");
            Map(m => m.AvSp).Name("AvSp");
            Map(m => m.Dir).Name("Dir");
            Map(m => m.MxS).Name("MxS");
            Map(m => m.SkyC).Name("SkyC");
            Map(m => m.MxR).Name("MxR");
            Map(m => m.Mn).Name("Mn");
            Map(m => m.R).Name("R AvSLP").TypeConverter(new SplitField<double>(0));
            Map(m => m.AvSLP).Name("R AvSLP").TypeConverter(new SplitField<double>(1));
        }
    }
}