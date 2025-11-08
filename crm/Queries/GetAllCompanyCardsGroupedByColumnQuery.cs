using Dtos.CompanyCardDtos;
using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries
{
    public class CompanyCardsByColumnDto
    {
        public string Name { get; set; }
        public Guid Id { get; set; }
        public List<Dtos.CompanyCardDtos.CompanyCardDetailsDto> Cards { get; set; }
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
                .Include(card => card.User)
                .Include(card => card.Company)
                .Include(card => card.StepColumn)
                .GroupBy(card => card.StepColumnId)
                .Select(group => new CompanyCardsByColumnDto
                {
                    Name = group.First().StepColumn.Name,
                    Id = group.Key,
                    Cards = group.Select(card => new Dtos.CompanyCardDtos.CompanyCardDetailsDto
                    {
                        Id = card.Id,
                        UserId = card.UserId,
                        UserName = card.User.Name,
                        CompanyId = card.CompanyId,
                        CompanyName = card.Company.Name,
                        StepColumnId = card.StepColumnId,
                        StepColumnName = card.StepColumn.Name
                    }).ToList()
                })
                .ToListAsync(cancellationToken);

            return groupedCards;
        }
    }
}
