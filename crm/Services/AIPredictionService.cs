using System.Text;
using System.Text.Json;
using Dtos.Predict;
using Interfaces;

namespace Services
{
    public class AIPredictionService : IAIPredictionService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AIPredictionService> _logger;

        public AIPredictionService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<AIPredictionService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<AIPredictResponseDto> PredictAsync(AIPredictRequestDto request)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var aiApiUrl = _configuration["AIApi:BaseUrl"] ?? "http://localhost:8000";
                var endpoint = $"{aiApiUrl}/predict";

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation("Calling AI API at {Endpoint} with data: {Data}", endpoint, json);

                var response = await client.PostAsync(endpoint, content);
                var responseString = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("AI API returned error: {StatusCode} - {Response}", 
                        response.StatusCode, responseString);
                    throw new HttpRequestException($"AI API error: {responseString}");
                }

                var predictResponse = JsonSerializer.Deserialize<AIPredictResponseDto>(
                    responseString, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                _logger.LogInformation("AI API prediction result: {Result}", predictResponse?.result);

                return predictResponse ?? throw new Exception("Empty response from AI API");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling AI prediction service");
                throw;
            }
        }
    }
}