using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Dtos.CompanyDtos;

namespace Commands
{
    public class RegisterCompanyCommand : IRequest<RegisterCompanyCommandResponse>
    {
        [Required(ErrorMessage = "Nome da empresa é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "CNPJ é obrigatório")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cliente é obrigatório")]
        public Guid ClientId { get; set; }

        public string? CompanyPicture { get; set; }
    }

    public class RegisterCompanyCommandResponse
    {
        public Guid CompanyId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Cnpj { get; set; } = string.Empty;
        public Guid ClientId { get; set; }
        public string? CompanyPicture { get; set; }
        public string? Message { get; set; }
    }

    public class RegisterCompanyCommandHandler : IRequestHandler<RegisterCompanyCommand, RegisterCompanyCommandResponse>
    {
        private readonly AppDbContext _context;

        public RegisterCompanyCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegisterCompanyCommandResponse> Handle(RegisterCompanyCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Company.AnyAsync(c => c.Cnpj == request.Cnpj, cancellationToken))
                throw new Exception("CNPJ já cadastrado");

            var company = new CompanyModel(request.Name, request.Cnpj, request.ClientId, request.CompanyPicture);
            _context.Company.Add(company);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterCompanyCommandResponse
            {
                CompanyId = company.Id,
                Name = company.Name,
                Cnpj = company.Cnpj,
                ClientId = company.ClientId,
                CompanyPicture = company.CompanyPicture,
                Message = "Empresa cadastrada com sucesso"
            };
        }
    }
}
