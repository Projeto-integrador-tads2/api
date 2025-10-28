using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos.CompanyCardDtos;

namespace Queries
{
    public class GetAllCompanyCardsQuery : IRequest<List<CompanyCardDto>>
    {
    }

    public class GetAllCompanyCardsQueryHandler : IRequestHandler<GetAllCompanyCardsQuery, List<CompanyCardDto>>
    {
        private readonly AppDbContext _context;

        public GetAllCompanyCardsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyCardDto>> Handle(GetAllCompanyCardsQuery request, CancellationToken cancellationToken)
        {
            var companyCards = await _context.Cards.ToListAsync(cancellationToken);

            return companyCards.Select(companyCard => new CompanyCardDto
            {
                UserId = companyCard.UserId,
                CompanyId = companyCard.CompanyId,
                StepColumnId = companyCard.StepColumnId
            }).ToList();
        }
    }
}
