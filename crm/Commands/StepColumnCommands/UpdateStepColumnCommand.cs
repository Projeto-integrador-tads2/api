using MediatR;
using Models;
using Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Commands.StepColumnCommands
{
    public class UpdateStepColumnCommand : IRequest<UpdateStepColumnCommandResponse>
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Cor é obrigatória")]
        public string Color { get; set; }
    }

    public class UpdateStepColumnCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        public string? Message { get; set; }
    }

    public class UpdateStepColumnCommandHandler : IRequestHandler<UpdateStepColumnCommand, UpdateStepColumnCommandResponse>
    {
        private readonly AppDbContext _context;

        public UpdateStepColumnCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateStepColumnCommandResponse> Handle(UpdateStepColumnCommand request, CancellationToken cancellationToken)
        {
            var stepColumn = await _context.StepColumn.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (stepColumn == null)
                throw new Exception("Coluna não encontrada");

            stepColumn.Update(request.Name, request.Color);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateStepColumnCommandResponse
            {
                Id = stepColumn.Id,
                Name = stepColumn.Name,
                Order = stepColumn.Order,
                Color = stepColumn.Color,
                IsActive = stepColumn.IsActive,
                Message = "Coluna atualizada com sucesso"
            };
        }
    }
}
