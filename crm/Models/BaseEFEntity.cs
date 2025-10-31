using System.ComponentModel.DataAnnotations;

namespace Models
{
    public abstract class BaseEFEntity
    {
        private static readonly TimeZoneInfo BrasiliaTimeZone = GetBrasiliaTimeZone();

        [Key]
        public Guid Id { get; protected set; } = Guid.NewGuid();
        public DateTime CreatedAt { get; protected set; } = GetBrasiliaTime();
        public DateTime? UpdatedAt { get; protected set; }

        private static TimeZoneInfo GetBrasiliaTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("E. South America Standard Time");
            }
            catch (TimeZoneNotFoundException)
            {
                try
                {
                    return TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");
                }
                catch (TimeZoneNotFoundException)
                {
                    return TimeZoneInfo.CreateCustomTimeZone(
                        "Brasilia Standard Time",
                        TimeSpan.FromHours(-3),
                        "Brasilia Standard Time",
                        "Brasilia Standard Time"
                    );
                }
            }
        }

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
