using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    public class HistoryModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime MovedAt { get; set; }

        [Required]
        public string Moved_To { get; set; } = string.Empty;

        [ForeignKey("Moved_To")]
        public StepColumnModel StepColumn { get; set; } = null!;
        
        [Required]
        public string Card_Id { get; set; } = string.Empty;
        
        [ForeignKey("Card_Id")]
        public CompanyCardModel Card { get; set; } = null!;
        
    }
}