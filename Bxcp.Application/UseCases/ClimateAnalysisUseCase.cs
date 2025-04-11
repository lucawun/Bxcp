using Bxcp.Application.DTOs;
using Bxcp.Application.Exceptions;
using Bxcp.Application.Mappers;
using Bxcp.Application.Ports.Driving;
using Bxcp.Domain.Models;
using Bxcp.Domain.Ports;

namespace Bxcp.Application.UseCases;

public class ClimateAnalysisUsecase : IClimateAnalysisUsecase
{
    private readonly IDataProviderRepository<Weather> _repository;
    private readonly IClimateService _climateService;

    public ClimateAnalysisUsecase(
       IDataProviderRepository<Weather> repository,
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
            throw new AnalysisFailedException($"Failed to analyze Climate data: {ex.Message}", ex);
        }
    }
}
