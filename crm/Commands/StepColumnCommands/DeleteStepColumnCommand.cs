using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Commands.StepColumnCommands
{
    public class DeleteStepColumnCommand : IRequest<DeleteStepColumnCommandResponse>
    {
        public Guid Id { get; set; }
    }

    public class DeleteStepColumnCommandResponse
    {
        public string? Message { get; set; }
    }

    public class DeleteStepColumnCommandHandler : IRequestHandler<DeleteStepColumnCommand, DeleteStepColumnCommandResponse>
    {
        private readonly AppDbContext _context;

        public DeleteStepColumnCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteStepColumnCommandResponse> Handle(DeleteStepColumnCommand request, CancellationToken cancellationToken)
        {
            var stepColumn = await _context.StepColumn.FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);
            if (stepColumn == null)
                throw new Exception("Coluna não encontrada");

            _context.StepColumn.Remove(stepColumn);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteStepColumnCommandResponse
            {
                Message = "Coluna deletada com sucesso"
            };
        }
    }
}
