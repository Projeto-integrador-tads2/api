using MediatR;
using Data;
using Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class UpdateClientCommand : IRequest<UpdateClientCommandResponse>
    {
        [Required(ErrorMessage = "Id é obrigatório")]
        public string ClientId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Phone { get; set; } = string.Empty;
    }

    public class UpdateClientCommandResponse
    {
        public string ClientId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class UpdateClientCommandHandler : IRequestHandler<UpdateClientCommand, UpdateClientCommandResponse>
    {
        private readonly AppDbContext _context;

        public UpdateClientCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateClientCommandResponse> Handle(UpdateClientCommand request, CancellationToken cancellationToken)
        {
            var clientId = Guid.Parse(request.ClientId);
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);

            if (client == null)
                throw new Exception("Cliente não encontrado");

            var emailExists = await _context.Clients
                .AnyAsync(c => c.Email == request.Email && c.Id != clientId, cancellationToken);

            if (emailExists)
                throw new Exception("Email já está em uso por outro cliente");

            // Usa o método Update da própria classe ClientModel
            client.Update(request.Name, request.Email, request.Phone);

            _context.Clients.Update(client);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateClientCommandResponse
            {
                ClientId = client.Id.ToString(),
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyId = client.CompanyId.ToString(),
                Message = "Cliente atualizado com sucesso"
            };
        }
    }
}
