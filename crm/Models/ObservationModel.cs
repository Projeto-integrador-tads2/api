using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class ObservationModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int Order { get; set; }

        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Color { get; set; } = string.Empty;

        [Required]
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; } = null!;

        [Required]
        public Guid StepColum_Id { get; set; }

        [ForeignKey("StepColumn_Id")]
        public StepColumnModel StepColumn { get; set; } = null!;
    }
}
