using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries
{
    public class GetAllServicesQuery : IRequest<GetAllServicesQueryResponse>
    {
    }

    public class GetAllServicesQueryResponse
    {
        public List<ServiceResponseDto> Services { get; set; } = new List<ServiceResponseDto>();
    }

    public class ServiceResponseDto
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ServicePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, GetAllServicesQueryResponse>
    {
        private readonly AppDbContext _context;

        public GetAllServicesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllServicesQueryResponse> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            var services = await _context.Services
                .Select(s => new ServiceResponseDto
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    Description = s.Description,
                    ServicePicture = s.ServicePicture,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return new GetAllServicesQueryResponse
            {
                Services = services
            };
        }
    }
}
