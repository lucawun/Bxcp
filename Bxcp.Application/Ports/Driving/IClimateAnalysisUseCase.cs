using Bxcp.Application.DTOs;

namespace Bxcp.Application.Ports.Driving;

public interface IClimateAnalysisUseCase
{
    /// <summary>
    /// Analyzes weather data to find the day with the smallest temperature spread
    /// </summary>
    /// <returns>Analysis result containing the day with smallest temperature spread</returns>
    ClimateAnalysisResult AnalyzeClimate();
}