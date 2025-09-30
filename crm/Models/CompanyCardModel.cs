using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CompanyCardModel
    {
        [Key]
        private Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        private DateTime UpdatedAt { get; set; }

        [Required]
        private DateTime CreatedAt { get; set; }
        
        [Required]
        public Guid User_Id { get; set; }

        [ForeignKey("UserId")]
        public UserModel User { get; set; } = null!;
        
        [Required]
        public Guid StepColumn_Id { get; set; }

        [ForeignKey("StepColumn_Id")]
        public UserModel StepColumn { get; set; } = null!;
    }
}