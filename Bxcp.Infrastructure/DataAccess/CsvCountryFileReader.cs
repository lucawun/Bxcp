using Bxcp.Infrastructure.DTOs;

namespace Bxcp.Infrastructure.DataAccess;

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
    public override string[] GetRequiredColumns()
    {
        return new[] { "Name", "Capital" };
    }

    /// <inheritdoc />
    public override CsvCountryRecord ParseLine(CsvRawRecord rawRecord)
    {
        return new CsvCountryRecord
        {
            Name = rawRecord.GetString("Name"),
            Capital = rawRecord.GetString("Capital"),
            Accession = rawRecord.GetString("Accession"),
            Population = rawRecord.GetInt("Population"),
            Area = rawRecord.GetInt("Area (km²)"),
            GDP = rawRecord.GetString("GDP (US$ M)"),
            HDI = rawRecord.GetString("HDI"),
            MEPs = rawRecord.GetString("MEPs")
        };
    }
}