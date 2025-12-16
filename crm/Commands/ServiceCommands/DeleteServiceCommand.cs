using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Commands.ServiceCommands
{
    public class DeleteServiceCommand : IRequest<DeleteServiceCommandResponse>
    {
        public Guid ServiceId { get; set; }
    }

    public class DeleteServiceCommandResponse
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, DeleteServiceCommandResponse>
    {
        private readonly Data.AppDbContext _context;
        public DeleteServiceCommandHandler(Data.AppDbContext context) { _context = context; }
        public async Task<DeleteServiceCommandResponse> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);
            if (service == null)
                return new DeleteServiceCommandResponse { Success = false, Message = "Serviço não encontrado" };
            _context.Services.Remove(service);
            await _context.SaveChangesAsync(cancellationToken);
            return new DeleteServiceCommandResponse { Success = true, Message = "Serviço removido com sucesso" };
        }
    }
}