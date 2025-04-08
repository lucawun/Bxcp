namespace Bxcp.Domain.Exceptions;

/// <summary>
/// Exception thrown when data analysis fails
/// </summary>
public class AnalysisFailedException : DomainException
{
    public AnalysisFailedException(string message) : base(message) { }

    public AnalysisFailedException(string message, Exception innerException)
        : base(message, innerException) { }
}