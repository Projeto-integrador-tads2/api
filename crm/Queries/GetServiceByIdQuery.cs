using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries
{
    public class GetServiceByIdQuery : IRequest<GetServiceByIdQueryResponse>
    {
        public string ServiceId { get; set; } = string.Empty;
    }

    public class GetServiceByIdQueryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ServicePicture { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, GetServiceByIdQueryResponse>
    {
        private readonly AppDbContext _context;

        public GetServiceByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetServiceByIdQueryResponse> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var serviceId = Guid.Parse(request.ServiceId);
            var service = await _context.Services
                .Where(s => s.Id == serviceId)
                .Select(s => new GetServiceByIdQueryResponse
                {
                    Id = s.Id.ToString(),
                    Name = s.Name,
                    Description = s.Description,
                    ServicePicture = s.ServicePicture,
                    CreatedAt = s.CreatedAt,
                    UpdatedAt = s.UpdatedAt
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (service == null)
                throw new Exception("Serviço não encontrado");

            return service;
        }
    }
}
