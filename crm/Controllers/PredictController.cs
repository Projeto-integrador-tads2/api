using Microsoft.AspNetCore.Mvc;
using MediatR;
using Dtos.Predict;
using Queries;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<PredictController> _logger;

        public PredictController(IMediator mediator, ILogger<PredictController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        /// <summary>
        /// Obtém a predição de sucesso de um card usando IA
        /// </summary>
        /// <param name="request">Dados do card para predição</param>
        /// <returns>Probabilidade de sucesso do card</returns>
        [HttpPost]
        [ProducesResponseType(typeof(PredictResponseDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> Predict([FromBody] PredictRequestDto request)
        {
            try
            {
                var query = new GetCardPredictionQuery
                {
                    CompanyCardId = request.CompanyCardId
                };

                var result = await _mediator.Send(query);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Validation error for card {CardId}", request.CompanyCardId);
                return BadRequest(new { error = ex.Message });
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error communicating with AI API for card {CardId}", request.CompanyCardId);
                return StatusCode(500, new { error = "Erro ao comunicar com a API de IA", details = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error predicting card {CardId}", request.CompanyCardId);
                return StatusCode(500, new { error = "Erro interno ao processar predição" });
            }
        }
    }
}
