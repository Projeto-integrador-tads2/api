using System.ComponentModel.DataAnnotations;

namespace Dtos
{
    public class ServiceDto
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Description { get; set; } = string.Empty;

        public string? ServicePicture { get; set; }

        public IFormFile? Picture { get; set; }
        public string? CustomFileName { get; set; }
    }
}
