using MediatR;
using Dtos.ServiceDtos;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries.ServiceQueries
{
    public class GetServiceByIdQuery : IRequest<ServiceDto>
    {
        public Guid ServiceId { get; set; }
    }

    public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ServiceDto>
    {
        private readonly AppDbContext _context;
        public GetServiceByIdQueryHandler(AppDbContext context) { _context = context; }
        public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
        {
            var s = await _context.Services.FirstOrDefaultAsync(x => x.Id == request.ServiceId, cancellationToken);
            if (s == null) return null;
            return new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ContractDuration = s.ContractDuration,
                Value = s.Value,
                ServicePicture = s.ServicePicture
            };
        }
    }

    public class GetAllServicesQuery : IRequest<List<ServiceDto>> { }

    public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, List<ServiceDto>>
    {
        private readonly AppDbContext _context;
        public GetAllServicesQueryHandler(AppDbContext context) { _context = context; }
        public async Task<List<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Services.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description,
                ContractDuration = s.ContractDuration,
                Value = s.Value,
                ServicePicture = s.ServicePicture
            }).ToListAsync(cancellationToken);
        }
    }
}