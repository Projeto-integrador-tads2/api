using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Commands.CompanyCardCommands
{
    public class DeleteCompanyCardCommand : IRequest<DeleteCompanyCardCommandResponse>
    {
        public Guid CompanyCardId { get; set; }
    }

    public class DeleteCompanyCardCommandResponse
    {
        public string? Message { get; set; }
    }

    public class DeleteCompanyCardCommandHandler : IRequestHandler<DeleteCompanyCardCommand, DeleteCompanyCardCommandResponse>
    {
        private readonly AppDbContext _context;

        public DeleteCompanyCardCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteCompanyCardCommandResponse> Handle(DeleteCompanyCardCommand request, CancellationToken cancellationToken)
        {
            var companyCard = await _context.Cards
                .FirstOrDefaultAsync(cc => cc.Id == request.CompanyCardId, cancellationToken);

            if (companyCard == null)
                throw new Exception("Card de empresa não encontrado");

            _context.Cards.Remove(companyCard);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteCompanyCardCommandResponse
            {
                Message = "Card de empresa deletado com sucesso"
            };
        }
    }
}
