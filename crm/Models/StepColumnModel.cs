using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class StepColumnModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public int Order { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Color { get; set; } = string.Empty;
    }
}