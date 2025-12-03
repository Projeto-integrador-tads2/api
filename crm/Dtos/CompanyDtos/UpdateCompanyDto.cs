using System.ComponentModel.DataAnnotations;

namespace Dtos.CompanyDtos
{
    public class UpdateCompanyDto
    {
        [Required(ErrorMessage = "Nome da empresa é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        public string Cnpj { get; set; } = string.Empty;

        public string? CompanyPicture { get; set; }
    }
}
