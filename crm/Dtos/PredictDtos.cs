namespace Dtos.Predict
{
    public class PredictRequestDto
    {
        public Guid CompanyCardId { get; set; }
    }

    public class PredictResponseDto
    {
        public string Result { get; set; }
        public PredictDataDto Data { get; set; }
    }

    public class PredictDataDto
    {
        public string FromStage { get; set; }
        public string ToStage { get; set; }
        public int TotalMoves { get; set; }
        public int DaysSinceCreation { get; set; }
        public int CurrentStageDuration { get; set; }
        public float Value { get; set; }
        public string Sector { get; set; }
        public string Prioridade { get; set; }
    }

    // DTO para enviar para a API Python
    public class AIPredictRequestDto
    {
        public string from_stage { get; set; }
        public string to_stage { get; set; }
        public int total_moves { get; set; }
        public int days_since_creation { get; set; }
        public int current_stage_duration { get; set; }
        public float value { get; set; }
        public string sector { get; set; }
        public string prioridade { get; set; }
    }

    // DTO para receber da API Python
    public class AIPredictResponseDto
    {
        public string result { get; set; }
    }
}