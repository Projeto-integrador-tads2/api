using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class HistoryModel
    {
        [Key]
        private Guid Id { get; set; } = Guid.NewGuid();
        
    }
}