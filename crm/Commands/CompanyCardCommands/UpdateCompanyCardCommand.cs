using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands.CompanyCardCommands
{
    public class UpdateCompanyCardCommand : IRequest<UpdateCompanyCardCommandResponse>
    {
        public Guid CompanyCardId { get; set; }

        [Required(ErrorMessage = "Usuário é obrigatório")]
        public Guid UserId { get; set; }

        [Required(ErrorMessage = "Empresa é obrigatória")]
        public Guid CompanyId { get; set; }

        [Required(ErrorMessage = "Coluna é obrigatória")]
        public Guid StepColumnId { get; set; }
    }

    public class UpdateCompanyCardCommandResponse
    {
        public Guid CompanyCardId { get; set; }
        public Guid UserId { get; set; }
        public Guid CompanyId { get; set; }
        public Guid StepColumnId { get; set; }
        public string? Message { get; set; }
    }

    public class UpdateCompanyCardCommandHandler : IRequestHandler<UpdateCompanyCardCommand, UpdateCompanyCardCommandResponse>
    {
        private readonly AppDbContext _context;

        public UpdateCompanyCardCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateCompanyCardCommandResponse> Handle(UpdateCompanyCardCommand request, CancellationToken cancellationToken)
        {
            var companyCard = await _context.Cards
                .FirstOrDefaultAsync(cc => cc.Id == request.CompanyCardId, cancellationToken);

            if (companyCard == null)
                throw new Exception("Card de empresa não encontrado");

            // Verificar se o usuário existe
            if (!await _context.User.AnyAsync(u => u.Id == request.UserId, cancellationToken))
                throw new Exception("Usuário não encontrado");

            // Verificar se a empresa existe
            if (!await _context.Company.AnyAsync(c => c.Id == request.CompanyId, cancellationToken))
                throw new Exception("Empresa não encontrada");

            // Verificar se a coluna existe
            if (!await _context.StepColumn.AnyAsync(s => s.Id == request.StepColumnId, cancellationToken))
                throw new Exception("Coluna não encontrada");

            companyCard.Update(request.UserId, request.CompanyId, request.StepColumnId);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateCompanyCardCommandResponse
            {
                CompanyCardId = companyCard.Id,
                UserId = companyCard.UserId,
                CompanyId = companyCard.CompanyId,
                StepColumnId = companyCard.StepColumnId,
                Message = "Card de empresa atualizado com sucesso"
            };
        }
    }
}
