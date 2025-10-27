using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class ValidateTokenDto
    {
        [Required(ErrorMessage = "Token é obrigatório")]
        public string Token { get; set; } = string.Empty;
        
        public bool IsValid { get; set; }
        public string? Message { get; set; }
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
    }
}
