using Bxcp.Application.DTOs;
using Bxcp.Application.Exceptions;
using Bxcp.Application.Mappers;
using Bxcp.Application.Ports.Driving;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Application.UseCases;
public class CountryAnalysisStratisticsUsecase : ICountryAnalysisStatisticsUsecase
{
    private readonly IDataProviderRepository<Country> _repository;
    private readonly ICountryStatisticsService _countryStatisticsService;

    public CountryAnalysisStratisticsUsecase(
       IDataProviderRepository<Country> fileReader,
       ICountryStatisticsService countryStatisticsService)
    {
        _repository = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        _countryStatisticsService = countryStatisticsService ?? throw new ArgumentNullException(nameof(countryStatisticsService));
    }

    /// <inheritdoc />
    public CountryAnalysisResult AnalyzeCountryStatistics()
    {
        try
        {
            IEnumerable<Country> records = _repository.ReadAllRecords();

            if (records?.Any() != true)
            {
                throw new EmptyDataException("No records found in the file.");
            }

            Country country = _countryStatisticsService.FindHighestPopulationDensity(records);

            return CountryStatisticsMapper.ToCountryStatisticsResult(country);
        }
        catch (Exception ex)
        {
            throw new AnalysisFailedException($"Failed to analyze Country data: {ex.Message}", ex);
        }
    }
}
