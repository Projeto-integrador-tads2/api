using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos.StepColumnDtos;

namespace Queries
{
    public class GetStepColumnByIdQuery : IRequest<StepColumnDto>
    {
        public Guid Id { get; set; }
    }

    public class GetStepColumnByIdQueryHandler : IRequestHandler<GetStepColumnByIdQuery, StepColumnDto>
    {
        private readonly AppDbContext _context;

        public GetStepColumnByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<StepColumnDto> Handle(GetStepColumnByIdQuery request, CancellationToken cancellationToken)
        {
            var stepColumn = await _context.StepColumn.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (stepColumn == null)
                throw new Exception("Coluna não encontrada");

            return new StepColumnDto
            {
                Id = stepColumn.Id,
                Name = stepColumn.Name,
                Order = stepColumn.Order,
                Color = stepColumn.Color,
                IsActive = stepColumn.IsActive
            };
        }
    }
}
