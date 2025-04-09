using Bxcp.Application.DTOs;

namespace Bxcp.Application.Ports.Incoming;

public interface ICountryAnalysisStatisticsUsecase
{
    /// <summary>
    /// Analyzes country data to find the country with the highest population density
    /// </summary>
    /// <returns>Analysis result containing the country with highest population density</returns>
    CountryAnalysisResult AnalyzeCountryStatistics();
}