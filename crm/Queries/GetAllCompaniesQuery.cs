using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos.CompanyDtos;

namespace Commands
{
    public class GetAllCompaniesQuery : IRequest<List<CompanyDto>>
    {
    }

    public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyDto>>
    {
        private readonly AppDbContext _context;

        public GetAllCompaniesQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CompanyDto>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _context.Company.ToListAsync(cancellationToken);
            return companies.Select(company => new CompanyDto
            {
                CompanyId = company.Id,
                Name = company.Name,
                Cnpj = company.Cnpj,
                ClientId = company.ClientId,
                CompanyPicture = company.CompanyPicture
            }).ToList();
        }
    }
}
