using CsvHelper;
using CsvHelper.Configuration;

using Bxcp.Infrastructure.DTOs;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper;

/// <summary>
/// Reads country data from CSV files
/// </summary>
public class CsvCountryFileReader : CsvBaseFileReader<CsvCountryRecord>
{
    /// <summary>
    /// Initializes a new instance of the CsvCountryFileReader
    /// </summary>
    /// <param name="filePath">Path to the CSV file containing country data</param>
    public CsvCountryFileReader(string filePath) : base(filePath, ';')
    {
    }

    /// <inheritdoc />
    protected override void RegisterMapping(CsvReader csvReader)
    {
        csvReader.Context.RegisterClassMap<CountryRecordMap>();
    }

    /// <summary>
    /// Maps CSV columns to CsvCountryRecord DTO
    /// </summary>
    private class CountryRecordMap : ClassMap<CsvCountryRecord>
    {
        public CountryRecordMap()
        {
            Map(m => m.Name).Name("Name");
            Map(m => m.Capital).Name("Capital");
            Map(m => m.Accession).Name("Accession");
            Map(m => m.Population).Name("Population");
            Map(m => m.Area).Name("Area (km²)");
            Map(m => m.GDP).Name("GDP (US$ M)");
            Map(m => m.HDI).Name("HDI");
            Map(m => m.MEPs).Name("MEPs");
        }
    }
}