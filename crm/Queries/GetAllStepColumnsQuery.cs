using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos.StepColumnDtos;

namespace Queries
{
    public class GetAllStepColumnsQuery : IRequest<List<StepColumnDto>>
    {
    }

    public class GetAllStepColumnsQueryHandler : IRequestHandler<GetAllStepColumnsQuery, List<StepColumnDto>>
    {
        private readonly AppDbContext _context;

        public GetAllStepColumnsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<StepColumnDto>> Handle(GetAllStepColumnsQuery request, CancellationToken cancellationToken)
        {
            var stepColumns = await _context.StepColumn.ToListAsync(cancellationToken);

            return stepColumns.Select(stepColumn => new StepColumnDto
            {
                Id = stepColumn.Id,
                Name = stepColumn.Name,
                Order = stepColumn.Order,
                Color = stepColumn.Color,
                IsActive = stepColumn.IsActive
            }).ToList();
        }
    }
}
