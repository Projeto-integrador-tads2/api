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
        public string Color { get; set; }
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
            var columns = await _context.StepColumn.ToListAsync(cancellationToken);
            var cards = await _context.Cards
                .Include(card => card.User)
                .Include(card => card.Company)
                .Include(card => card.StepColumn)
                .ToListAsync(cancellationToken);

            var grouped = columns
                .GroupJoin(
                    cards,
                    column => column.Id,
                    card => card.StepColumnId,
                    (column, cardsGroup) => new CompanyCardsByColumnDto
                    {
                        Name = column.Name,
                        Id = column.Id,
                        Color = column.Color,
                        Cards = cardsGroup.Select(card => new Dtos.CompanyCardDtos.CompanyCardDetailsDto
                        {
                            Id = card.Id,
                            UserId = card.UserId,
                            UserName = card.User?.Name,
                            CompanyId = card.CompanyId,
                            CompanyName = card.Company?.Name,
                            StepColumnId = card.StepColumnId,
                            StepColumnName = card.StepColumn?.Name
                        }).ToList()
                    }
                )
                .ToList();

            return grouped;
        }
    }
}
