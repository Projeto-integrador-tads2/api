using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class ClientDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Empresa é obrigatória")]
        public string CompanyId { get; set; } = string.Empty;
    }
}
