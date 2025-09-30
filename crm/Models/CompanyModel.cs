using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CompanyModel
    {
        [Key]
        private Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Cnpj { get; set; } = string.Empty;

        [Required]
        private DateTime UpdatedAt { get; set; }

        [Required]
        private DateTime CreatedAt { get; set; }

        [Required]
        public Guid Client_Id { get; set; }

        [ForeignKey("ClientId")]
        public ClientModel Client { get; set; } = null!;
        
    }
}