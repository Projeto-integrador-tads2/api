namespace Dtos
{
    public class FileUploadDto
    {
        // Para Request
        public IFormFile? File { get; set; }
        public string? CustomFileName { get; set; }
        
        // Para Response  
        public string? FileUrl { get; set; }
        public string? FileName { get; set; }
        public long? FileSize { get; set; }
        public string? ContentType { get; set; }
        public DateTime? UploadedAt { get; set; }
    }
}
