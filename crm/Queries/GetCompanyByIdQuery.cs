using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos.CompanyDtos;

namespace Commands
{
    public class GetCompanyByIdQuery : IRequest<CompanyDto>
    {
        public Guid CompanyId { get; set; }
    }

    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyDto>
    {
        private readonly AppDbContext _context;

        public GetCompanyByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {
            var company = await _context.Company.FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
            if (company == null)
                throw new Exception("Empresa não encontrada");

            return new CompanyDto
            {
                CompanyId = company.Id,
                Name = company.Name,
                Cnpj = company.Cnpj,
                CompanyPicture = company.CompanyPicture
            };
        }
    }
}
