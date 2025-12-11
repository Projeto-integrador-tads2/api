using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PredictController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public PredictController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        public class PredictRequest
        {
            public Guid CompanyCardId { get; set; }
        }

        public class PredictResponse
        {
            public string result { get; set; }
        }

        [HttpPost]
        public async Task<IActionResult> Predict([FromBody] PredictRequest request)
        {
            // Buscar dados do card e relacionados
            // Supondo que você tenha um AppDbContext injetado (adicione via DI se necessário)
            var db = (Data.AppDbContext)HttpContext.RequestServices.GetService(typeof(Data.AppDbContext));
            var card = db.Cards
                .Include(c => c.Company)
                .Include(c => c.StepColumn)
                .FirstOrDefault(c => c.Id == request.CompanyCardId);
            if (card == null)
                return NotFound("Card não encontrado");


            // Buscar histórico de movimentações
            var histories = db.Histories
                .Where(h => h.CompanyCardId == card.Id)
                .OrderBy(h => h.MovedAt)
                .ToList();

            // from_stage: penúltima coluna (ou a primeira se só houver uma)
            string from_stage = histories.Count > 1 ?
                db.StepColumn.FirstOrDefault(s => s.Id == histories[histories.Count - 2].ToStepColumnId)?.Name ?? card.StepColumn.Name :
                card.StepColumn.Name;

            // to_stage: última coluna
            string to_stage = card.StepColumn.Name;

            // total_moves: quantidade de movimentações
            int total_moves = histories.Count;

            // days_since_creation: dias desde a criação do card
            int days_since_creation = (int)(DateTime.UtcNow - card.CreatedAt).TotalDays;

            // current_stage_duration: dias desde a última movimentação (ou desde a criação se nunca movido)
            DateTime lastMove = histories.Count > 0 ? histories[histories.Count - 1].MovedAt : card.CreatedAt;
            int current_stage_duration = (int)(DateTime.UtcNow - lastMove).TotalDays;

            // value: valor do card (se existir campo, senão coloque 0 ou ajuste para o seu modelo)
            float value = 0; // Ajuste se houver campo de valor

            // sector: setor do cliente
            string sector = card.Company?.Sector ?? "";

            // prioridade: prioridade do card
            string prioridade = card.Priority ?? "";

            // --- Validação dos campos antes de chamar a IA ---
            var validStages = new[] { "Análise de Perfil", "Conversa com o Cliente", "Fechamento", "Negociação" };
            var validSectors = new[] { "Comércio", "Educação", "Indústria", "Saúde", "Serviços", "Tecnologia" };

            var errors = new List<string>();
            if (!validStages.Contains(from_stage))
                errors.Add($"from_stage deve ser um dos: [{string.Join(", ", validStages)}] (recebido: '{from_stage}')");
            if (!validStages.Contains(to_stage))
                errors.Add($"to_stage deve ser um dos: [{string.Join(", ", validStages)}] (recebido: '{to_stage}')");
            if (!validSectors.Contains(sector))
                errors.Add($"sector deve ser um dos: [{string.Join(", ", validSectors)}] (recebido: '{sector}')");

            if (errors.Count > 0)
                return BadRequest(new { detail = string.Join("; ", errors) });

            // Montar JSON esperado pela IA
            var iaRequest = new
            {
                from_stage,
                to_stage,
                total_moves,
                days_since_creation,
                current_stage_duration,
                value,
                sector,
                prioridade
            };

            var client = _httpClientFactory.CreateClient();
            var aiApiUrl = "http://localhost:8000/predict";
            var json = JsonSerializer.Serialize(iaRequest);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(aiApiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, error);
            }
            var responseString = await response.Content.ReadAsStringAsync();
            var predictResponse = JsonSerializer.Deserialize<PredictResponse>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return Ok(predictResponse);
        }
    }
}
