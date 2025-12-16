using Dtos.Predict;

namespace Interfaces
{
    public interface IAIPredictionService
    {
        Task<AIPredictResponseDto> PredictAsync(AIPredictRequestDto request);
    }
}