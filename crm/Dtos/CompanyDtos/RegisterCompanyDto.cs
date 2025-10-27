using System.ComponentModel.DataAnnotations;

namespace Dtos.CompanyDtos
{
    public class RegisterCompanyDto
    {
        [Required(ErrorMessage = "Nome da empresa é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cliente é obrigatório")]
        public Guid ClientId { get; set; }

        public string? CompanyPicture { get; set; }

        public Guid? CompanyId { get; set; }
        public string? Message { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
