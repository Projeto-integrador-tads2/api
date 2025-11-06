using Dtos.CompanyCardDtos;
using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries
{
    public class GetCompanyCardsByColumnIdQuery : IRequest<List<CompanyCardDto>>
    {
        public Guid ColumnId { get; set; }
    }
    public class GetCompanyCardsByColumnIdQueryHandler : IRequestHandler<GetCompanyCardsByColumnIdQuery, List<CompanyCardDto>>
    {
        private readonly AppDbContext _context;

        public GetCompanyCardsByColumnIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyCardDto>> Handle(GetCompanyCardsByColumnIdQuery request, CancellationToken cancellationToken)
        {
            var companyCards = await _context.Cards
                .Where(companyCard => companyCard.StepColumnId == request.ColumnId)
                .ToListAsync(cancellationToken);
            
            if (companyCards == null || !companyCards.Any())
                throw new Exception("Nenhum card de empresa encontrado para a coluna especificada");

            return companyCards.Select(companyCard => new CompanyCardDto
            {
                UserId = companyCard.UserId,
                CompanyId = companyCard.CompanyId,
                StepColumnId = companyCard.StepColumnId
            }).ToList();
        }
    }
}