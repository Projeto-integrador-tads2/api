using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        public string Password { get; set; } = string.Empty;
        
        // Propriedades de resposta (opcionais no request)
        public string? Token { get; set; }
        public Guid? UserId { get; set; }
        public string? Name { get; set; }
        public string? Message { get; set; }
    }
}
