using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class ForgotPasswordDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        
        // Propriedades de resposta (opcionais no request)
        public string? Message { get; set; }
        public bool? Success { get; set; }
    }
}
