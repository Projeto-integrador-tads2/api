using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class DeleteClientCommand : IRequest<DeleteClientCommandResponse>
    {
        [Required(ErrorMessage = "Id é obrigatório")]
        public string ClientId { get; set; } = string.Empty;
    }

    public class DeleteClientCommandResponse
    {
        public string ClientId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class DeleteClientCommandHandler : IRequestHandler<DeleteClientCommand, DeleteClientCommandResponse>
    {
        private readonly AppDbContext _context;

        public DeleteClientCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<DeleteClientCommandResponse> Handle(DeleteClientCommand request, CancellationToken cancellationToken)
        {
            var clientId = Guid.Parse(request.ClientId);
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);

            if (client == null)
                throw new Exception("Cliente não encontrado");

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync(cancellationToken);

            return new DeleteClientCommandResponse
            {
                ClientId = request.ClientId,
                Message = "Cliente deletado com sucesso"
            };
        }
    }
}
