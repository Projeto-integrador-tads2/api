using System;

namespace Dtos
{
    public class ObservationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid CompanyCardId { get; set; }
    }

    public class RegisterObservationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid CompanyCardId { get; set; }
    }

    public class UpdateObservationDto
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}