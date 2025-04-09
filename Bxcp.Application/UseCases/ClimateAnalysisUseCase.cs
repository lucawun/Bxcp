using Bxcp.Application.DTOs;
using Bxcp.Application.Mappers;
using Bxcp.Application.Ports.Incoming;
using Bxcp.Application.Ports.Outgoing;
using Bxcp.Domain.DomainServices.Ports;
using Bxcp.Domain.Exceptions;
using Bxcp.Domain.Models;

namespace Bxcp.Application.Services;

public class ClimateAnalysisUsecase : IClimateAnalysisUsecase
{
    private readonly IRepository<Weather> _repository;
    private readonly IClimateService _climateService;

    public ClimateAnalysisUsecase(
       IRepository<Weather> repository,
       IClimateService climateService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _climateService = climateService ?? throw new ArgumentNullException(nameof(climateService));
    }

    public ClimateAnalysisResult AnalyzeClimate()
    {
        try
        {
            IEnumerable<Weather> records = _repository.ReadAllRecords();

            if (records == null || !records.Any())
            {
                throw new EmptyDataException("No records found in the file.");
            }

            Weather weather = _climateService.FindSmallestTemperatureSpread(records);

            return ClimateAnalysisMapper.ToClimateAnalysisResult(weather);
        }
        catch (Exception ex)
        {
            throw new AnalysisFailedException($"Failed to analyze country data: {ex.Message}", ex);
        }
    }
}
