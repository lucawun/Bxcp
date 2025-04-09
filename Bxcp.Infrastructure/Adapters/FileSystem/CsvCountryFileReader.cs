using Bxcp.Infrastructure.DTOs.FileSystem;

namespace Bxcp.Infrastructure.Adapters.FileSystem;

/// <summary>
/// Reads country data from CSV files
/// </summary>
public class CsvCountryFileReader : BaseCsvFileReader<CsvCountryRecord>
{
    public CsvCountryFileReader(string filePath) : base(filePath, ';')
    {
    }

    /// <inheritdoc />
    internal override string[] GetRequiredColumns()
    {
        return new[] { "Name", "Capital" };
    }

    /// <inheritdoc />
    internal override CsvCountryRecord ParseLine(CsvRawRecord rawRecord)
    {
        return new CsvCountryRecord
        {
            Name = rawRecord.GetString("Name"),
            Capital = rawRecord.GetString("Capital"),
            Accession = rawRecord.GetString("Accession"),
            Population = rawRecord.GetString("Population"),
            Area = rawRecord.GetString("Area (km²)"),
            GDP = rawRecord.GetString("GDP (US$ M)"),
            HDI = rawRecord.GetString("HDI"),
            MEPs = rawRecord.GetString("MEPs")
        };
    }
}