using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos;

namespace Queries
{
    public class GetObservationByIdQuery : IRequest<ObservationDto>
    {
        public Guid ObservationId { get; set; }
    }

    public class GetObservationByIdQueryHandler : IRequestHandler<GetObservationByIdQuery, ObservationDto>
    {
        private readonly AppDbContext _context;

        public GetObservationByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ObservationDto> Handle(GetObservationByIdQuery request, CancellationToken cancellationToken)
        {
            var observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == request.ObservationId, cancellationToken);
            if (observation == null)
                throw new Exception("Observação não encontrada");

            return new ObservationDto
            {
                Id = observation.Id,
                Title = observation.Title,
                Content = observation.Content,
                UserId = observation.UserId,
                CompanyCardId = observation.CompanyCardId
            };
        }
    }

    public class GetAllObservationsQuery : IRequest<List<ObservationDto>>
    {
        public Guid? CompanyCardId { get; set; }
    }

    public class GetAllObservationsQueryHandler : IRequestHandler<GetAllObservationsQuery, List<ObservationDto>>
    {
        private readonly AppDbContext _context;

        public GetAllObservationsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<ObservationDto>> Handle(GetAllObservationsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Observations.AsQueryable();
            if (request.CompanyCardId.HasValue)
                query = query.Where(o => o.CompanyCardId == request.CompanyCardId.Value);

            var observations = await query.ToListAsync(cancellationToken);
            return observations.Select(o => new ObservationDto
            {
                Id = o.Id,
                Title = o.Title,
                Content = o.Content,
                UserId = o.UserId,
                CompanyCardId = o.CompanyCardId
            }).ToList();
        }
    }
}