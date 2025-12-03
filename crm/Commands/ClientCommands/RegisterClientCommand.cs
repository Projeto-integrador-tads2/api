using MediatR;
using Data;
using Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class RegisterClientCommand : IRequest<RegisterClientCommandResponse>
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Empresa é obrigatória")]
        public string CompanyId { get; set; } = string.Empty;
    }

    public class RegisterClientCommandResponse
    {
        public string ClientId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
    }

    public class RegisterClientCommandHandler : IRequestHandler<RegisterClientCommand, RegisterClientCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<object> _passwordHasher;

        public RegisterClientCommandHandler(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<RegisterClientCommandResponse> Handle(RegisterClientCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Clients.AnyAsync(u => u.Email == request.Email, cancellationToken))
                throw new Exception("Email já cadastrado");

            var companyId = Guid.Parse(request.CompanyId);
            var companyExists = await _context.Company.AnyAsync(c => c.Id == companyId, cancellationToken);
            if (!companyExists)
                throw new Exception("Empresa não encontrada");

            var client = new ClientModel(request.Name, request.Email, request.Phone, companyId);

            _context.Clients.Add(client);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterClientCommandResponse
            {
                ClientId = client.Id.ToString(),
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyId = client.CompanyId.ToString(),
                Message = "Cliente registrado com sucesso"
            };
        }
    }
}
