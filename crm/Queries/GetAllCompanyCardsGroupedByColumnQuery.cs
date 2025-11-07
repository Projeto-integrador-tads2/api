using Dtos.CompanyCardDtos;
using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries
{
    public class CompanyCardsByColumnDto
    {
        public Guid StepColumnId { get; set; }
        public List<CompanyCardDto> Cards { get; set; }
    }

    public class GetAllCompanyCardsGroupedByColumnQuery : IRequest<List<CompanyCardsByColumnDto>> { }

    public class GetAllCompanyCardsGroupedByColumnQueryHandler : IRequestHandler<GetAllCompanyCardsGroupedByColumnQuery, List<CompanyCardsByColumnDto>>
    {
        private readonly AppDbContext _context;

        public GetAllCompanyCardsGroupedByColumnQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyCardsByColumnDto>> Handle(GetAllCompanyCardsGroupedByColumnQuery request, CancellationToken cancellationToken)
        {
            var groupedCards = await _context.Cards
                .GroupBy(card => card.StepColumnId)
                .Select(group => new CompanyCardsByColumnDto
                {
                    StepColumnId = group.Key,
                    Cards = group.Select(card => new CompanyCardDto
                    {
                        UserId = card.UserId,
                        CompanyId = card.CompanyId,
                        StepColumnId = card.StepColumnId
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return groupedCards;
        }
    }
}
