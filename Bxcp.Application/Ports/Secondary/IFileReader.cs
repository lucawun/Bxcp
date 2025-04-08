namespace Bxcp.Application.Ports.Secondary;

/// <summary>
/// Secondary port for reading data from files
/// This interface will be implemented by adapters in the infrastructure layer
/// </summary>
/// <typeparam name="T">The type of data to be read from the file</typeparam>
public interface IFileReader<T>
{
    /// <summary>
    /// Reads all records from a file
    /// </summary>
    /// <param name="filePath">Path to the file to read</param>
    /// <returns>An enumerable collection of records</returns>
    IEnumerable<T> ReadAllRecords(string filePath);
}