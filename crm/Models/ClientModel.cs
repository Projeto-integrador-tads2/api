using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ClientModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
    
        [Required]
        public Guid ClientId { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Phone { get; set; } = string.Empty;

        [Required]
        private DateTime UpdatedAt { get; set; }

        [Required]
        private DateTime CreatedAt { get; set; }
    }
}