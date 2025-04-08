namespace Bxcp.Domain.Exceptions;

/// <summary>
/// Exception thrown when no data is available for analysis
/// </summary>
public class EmptyDataException : DomainException
{
    public EmptyDataException(string message) : base(message) { }

    public EmptyDataException(string message, Exception innerException)
        : base(message, innerException) { }
}
