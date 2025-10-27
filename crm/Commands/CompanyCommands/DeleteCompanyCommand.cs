using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Commands
{
    public class DeleteCompanyCommand : IRequest<DeleteCompanyCommandResponse>
    {
        public Guid CompanyId { get; set; }
    }

    public class DeleteCompanyCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, DeleteCompanyCommandResponse>
    {
        private readonly AppDbContext _context;

        public DeleteCompanyCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCompanyCommandResponse> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var company = await _context.Company.FirstOrDefaultAsync(c => c.Id == request.CompanyId, cancellationToken);
            if (company == null)
                return new DeleteCompanyCommandResponse { Success = false, Message = "Empresa não encontrada" };

            _context.Company.Remove(company);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteCompanyCommandResponse { Success = true, Message = "Empresa removida com sucesso" };
        }
    }
}
