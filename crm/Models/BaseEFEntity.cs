using System.ComponentModel.DataAnnotations;

namespace Models
{
    public abstract class BaseEFEntity
    {
        private static readonly TimeZoneInfo BrasiliaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");

        [Key]
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = GetBrasiliaTime();
        public DateTime? UpdatedAt { get; protected set; }

        private static DateTime GetBrasiliaTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, BrasiliaTimeZone);
        }

        protected void SetUpdatedAt()
        {
            UpdatedAt = GetBrasiliaTime();
        }
    }
}
