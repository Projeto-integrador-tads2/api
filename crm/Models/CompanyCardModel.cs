using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class CompanyCardModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }
        
        [Required]
        public Guid User_Id { get; set; }

        [ForeignKey("User_Id")]
        public UserModel User { get; set; } = null!;
        
        [Required]
        public Guid StepColumn_Id { get; set; }

        [ForeignKey("StepColumn_Id")]
        public StepColumnModel StepColumn { get; set; } = null!;
    }
}