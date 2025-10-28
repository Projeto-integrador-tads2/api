using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos.CompanyCardDtos;

namespace Queries
{
    public class GetCompanyCardByIdQuery : IRequest<CompanyCardDto>
    {
        public Guid CompanyCardId { get; set; }
    }

    public class GetCompanyCardByIdQueryHandler : IRequestHandler<GetCompanyCardByIdQuery, CompanyCardDto>
    {
        private readonly AppDbContext _context;

        public GetCompanyCardByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyCardDto> Handle(GetCompanyCardByIdQuery request, CancellationToken cancellationToken)
        {
            var companyCard = await _context.Cards
                .FirstOrDefaultAsync(cc => cc.Id == request.CompanyCardId, cancellationToken);

            if (companyCard == null)
                throw new Exception("Card de empresa não encontrado");

            return new CompanyCardDto
            {
                UserId = companyCard.UserId,
                CompanyId = companyCard.CompanyId,
                StepColumnId = companyCard.StepColumnId
            };
        }
    }
}
