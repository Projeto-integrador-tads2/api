using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class StepColumnModel
    {
        [Key]
        private Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        private DateTime CreatedAt { get; set; }
        [Required]
        private string Name { get; set; } = string.Empty;
        [Required]
        private DateTime UpdatedAt { get; set; }
        [Required]
        private int Order { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        private string Color { get; set; } = string.Empty;
    }
}