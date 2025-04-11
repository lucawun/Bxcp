using Bxcp.Infrastructure.DataAccess.CsvHelper.Utils.Converters;
using CsvHelper.TypeConversion;

namespace Bxcp.Infrastructure.DataAccess.CsvHelper.Utils;

/// <summary>
/// Factory to create type converters based on type
/// </summary>
public static class MultiFormatFactory
{
    public static ITypeConverter GetConverter(Type type)
    {
        if (type == typeof(int))
            return new MultiFormatInt();
        else if (type == typeof(double))
            return new MultiFormatDouble();
        else if (type == typeof(decimal))
            return new MultiFormatDecimal();
        else
            return new StringConverter();  // Default to string converter
    }
}