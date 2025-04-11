namespace Bxcp.Domain.Ports;

/// <summary>
/// This interface will be implemented by adapters in the infrastructure layer
/// </summary>
/// <typeparam name="T">The type of data to be read</typeparam>
public interface IDataProviderRepository<T>
{
    /// <summary>
    /// Reads all records
    /// </summary>
    /// <returns>An enumerable collection of records</returns>
    IEnumerable<T> ReadAllRecords();
}