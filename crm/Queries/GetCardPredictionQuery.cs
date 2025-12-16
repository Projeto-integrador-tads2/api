using MediatR;
using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.Predict;
using Interfaces;

namespace Queries
{
    // Query
    public class GetCardPredictionQuery : IRequest<PredictResponseDto>
    {
        public Guid CompanyCardId { get; set; }
    }

    // Handler
    public class GetCardPredictionQueryHandler : IRequestHandler<GetCardPredictionQuery, PredictResponseDto>
    {
        private readonly AppDbContext _context;
        private readonly IAIPredictionService _aiService;
        private readonly ILogger<GetCardPredictionQueryHandler> _logger;

        // Categorias válidas conforme o modelo Python
        private readonly string[] _validStages = { "Análise de Perfil", "Conversa com o Cliente", "Fechamento", "Negociação" };
        private readonly string[] _validSectors = { "Comércio", "Educação", "Indústria", "Saúde", "Serviços", "Tecnologia" };
        private readonly string[] _validPriorities = { "Alta", "Baixa", "Média" };

        public GetCardPredictionQueryHandler(
            AppDbContext context,
            IAIPredictionService aiService,
            ILogger<GetCardPredictionQueryHandler> logger)
        {
            _context = context;
            _aiService = aiService;
            _logger = logger;
        }

        public async Task<PredictResponseDto> Handle(GetCardPredictionQuery request, CancellationToken cancellationToken)
        {
            var card = await _context.Cards
                .Include(c => c.Company)
                .Include(c => c.StepColumn)
                .FirstOrDefaultAsync(c => c.Id == request.CompanyCardId, cancellationToken);

            if (card == null)
                throw new ArgumentException("Card não encontrado");

            var histories = await _context.Histories
                .Where(h => h.CompanyCardId == card.Id)
                .OrderBy(h => h.MovedAt)
                .ToListAsync(cancellationToken);

            var predictData = await PrepareData(card, histories, cancellationToken);

            ValidateData(predictData);

            var aiRequest = new AIPredictRequestDto
            {
                from_stage = predictData.FromStage,
                to_stage = predictData.ToStage,
                total_moves = predictData.TotalMoves,
                days_since_creation = predictData.DaysSinceCreation,
                current_stage_duration = predictData.CurrentStageDuration,
                value = predictData.Value,
                sector = predictData.Sector,
                prioridade = predictData.Prioridade
            };

            var aiResponse = await _aiService.PredictAsync(aiRequest);

            return new PredictResponseDto
            {
                Result = aiResponse.result,
                Data = predictData
            };
        }

        private async Task<PredictDataDto> PrepareData(
            Models.CompanyCardModel card, 
            List<Models.HistoryModel> histories,
            CancellationToken cancellationToken)
        {
            string fromStage = card.StepColumn.Name;
            if (histories.Count > 1)
            {
                var penultimateHistory = histories[histories.Count - 2];
                var fromColumn = await _context.StepColumn
                    .FirstOrDefaultAsync(s => s.Id == penultimateHistory.ToStepColumnId, cancellationToken);
                if (fromColumn != null)
                    fromStage = fromColumn.Name;
            }

            string toStage = card.StepColumn.Name;

            int totalMoves = histories.Count;

            int daysSinceCreation = (int)(DateTime.UtcNow - card.CreatedAt).TotalDays;

            DateTime lastMove = histories.Count > 0 ? histories[histories.Count - 1].MovedAt : card.CreatedAt;
            int currentStageDuration = (int)(DateTime.UtcNow - lastMove).TotalDays;

            float value = 0; 
            string sector = "Tecnologia";
            string prioridade = "Média"; 

            return new PredictDataDto
            {
                FromStage = fromStage,
                ToStage = toStage,
                TotalMoves = totalMoves,
                DaysSinceCreation = daysSinceCreation,
                CurrentStageDuration = currentStageDuration,
                Value = value,
                Sector = sector,
                Prioridade = prioridade
            };
        }

        private void ValidateData(PredictDataDto data)
        {
            var errors = new List<string>();

            if (!_validStages.Contains(data.FromStage))
                errors.Add($"from_stage deve ser um dos: [{string.Join(", ", _validStages)}] (recebido: '{data.FromStage}')");

            if (!_validStages.Contains(data.ToStage))
                errors.Add($"to_stage deve ser um dos: [{string.Join(", ", _validStages)}] (recebido: '{data.ToStage}')");

            if (!_validSectors.Contains(data.Sector))
                errors.Add($"sector deve ser um dos: [{string.Join(", ", _validSectors)}] (recebido: '{data.Sector}')");

            if (!_validPriorities.Contains(data.Prioridade))
                errors.Add($"prioridade deve ser um dos: [{string.Join(", ", _validPriorities)}] (recebido: '{data.Prioridade}')");

            if (errors.Count > 0)
            {
                var errorMessage = string.Join("; ", errors);
                _logger.LogWarning("Validation failed: {Errors}", errorMessage);
                throw new ArgumentException(errorMessage);
            }
        }
    }
}