namespace Dtos.ServiceDtos
{
    public class ServiceDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContractDuration { get; set; }
        public decimal Value { get; set; }
        public string? ServicePicture { get; set; }
    }

    public class RegisterServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContractDuration { get; set; }
        public decimal Value { get; set; }
        public string? ServicePicture { get; set; }
    }

    public class UpdateServiceDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ContractDuration { get; set; }
        public decimal Value { get; set; }
        public string? ServicePicture { get; set; }
    }
}