namespace Bxcp.Application.Exceptions;

/// <summary>
/// Exception thrown when data analysis fails
/// </summary>
public class AnalysisFailedException : ApplicationException
{
    public AnalysisFailedException(string message) : base(message) { }

    public AnalysisFailedException(string message, Exception innerException)
        : base(message, innerException) { }
}