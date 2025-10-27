namespace Dtos.CompanyDtos
{
    public class CompanyDto
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string? CompanyPicture { get; set; }
    }
}
