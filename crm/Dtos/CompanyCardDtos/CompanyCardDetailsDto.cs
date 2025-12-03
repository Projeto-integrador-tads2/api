namespace Dtos.CompanyCardDtos
{
    public class CompanyCardDetailsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid StepColumnId { get; set; }
        public string StepColumnName { get; set; }
    }
}