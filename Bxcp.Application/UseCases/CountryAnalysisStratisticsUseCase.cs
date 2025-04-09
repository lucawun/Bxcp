using Bxcp.Application.DTOs;
using Bxcp.Application.Mappers;
using Bxcp.Application.Ports.Incoming;
using Bxcp.Application.Ports.Outgoing;
using Bxcp.Domain.DomainServices.Ports;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;


namespace Bxcp.Application.Services;
public class CountryAnalysisStratisticsUsecase : ICountryAnalysisStatisticsUsecase
{
    private readonly IRepository<Country> _repository;
    private readonly ICountryStatisticsService _countryStatisticsService;

    public CountryAnalysisStratisticsUsecase(
       IRepository<Country> fileReader,
       ICountryStatisticsService countryStatisticsService)
    {
        _repository = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
        _countryStatisticsService = countryStatisticsService ?? throw new ArgumentNullException(nameof(countryStatisticsService));

    }

    public CountryAnalysisResult AnalyzeCountryStatistics()
    {
        try
        {
            IEnumerable<Country> records = _repository.ReadAllRecords();

            if (records == null || !records.Any())
            {
                throw new EmptyDataException("No records found in the file.");
            }

            Country country = _countryStatisticsService.FindHighestPopulationDensity(records);

            return CountryStatisticsMapper.ToCountryStatisticsResult(country);
        }
        catch (Exception ex)
        {
            throw new AnalysisFailedException($"Failed to analyze country data: {ex.Message}", ex);
        }
    }
}
