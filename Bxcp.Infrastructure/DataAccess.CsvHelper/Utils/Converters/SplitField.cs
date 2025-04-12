using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;

/// <summary>
/// Converter for splitting combined fields into parts
/// </summary>
public class SplitField<T> : DefaultTypeConverter
{
    private readonly int _partIndex;
    private readonly string _separator;
    private readonly ITypeConverter _innerConverter;

    public SplitField(int partIndex, string separator = " ")
    {
        _partIndex = partIndex;
        _separator = separator;
        _innerConverter = MultiFormatFactory.GetConverter(typeof(T));
    }

    public override object? ConvertFromString(string? text, IReaderRow row, MemberMapData memberMapData)
    {
        if (string.IsNullOrWhiteSpace(text))
            return _innerConverter.ConvertFromString(null, row, memberMapData);

        string[] parts = text.Split(_separator, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length > _partIndex)
        {
            return _innerConverter.ConvertFromString(parts[_partIndex], row, memberMapData);
        }

        // If part index is out of range, return default value for the type
        return _innerConverter.ConvertFromString(null, row, memberMapData);
    }
}