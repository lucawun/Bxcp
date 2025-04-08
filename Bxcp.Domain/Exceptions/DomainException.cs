
namespace Bxcp.Domain.Exceptions;

/// <summary>
/// A general exception for domain-related errors.
/// </summary>
public class DomainException : ApplicationException
{
    public DomainException(string message)
        : base(message) { }

    public DomainException(string message, Exception innerException)
        : base(message, innerException) { }
}