using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Dtos.CompanyDtos;

namespace Commands
{
    public class UpdateCompanyCommand : IRequest<UpdateCompanyCommandResponse>
    {
        [Required]
        public Guid CompanyId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Cnpj { get; set; } = string.Empty;
        public string? CompanyPicture { get; set; }
    }

    public class UpdateCompanyCommandResponse
    {
        public Guid CompanyId { get; set; }
        public string? Message { get; set; }
    }

    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, UpdateCompanyCommandResponse>
    {
        private readonly AppDbContext _context;

        public UpdateCompanyCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateCompanyCommandResponse> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Company.FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
            if (company == null)
                throw new Exception("Empresa não encontrada");

            company.Update(request.Name, request.Cnpj, request.CompanyPicture);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCompanyCommandResponse
            {
                CompanyId = company.Id,
                Message = "Empresa atualizada com sucesso"
            };
        }
    }
}
