using MediatR;
using Data;
using Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class RegisterServiceCommand : IRequest<RegisterServiceCommandResponse>
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Description { get; set; } = string.Empty;

        public string? ServicePicture { get; set; }
    }

    public class RegisterServiceCommandResponse
    {
        public string ServiceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ServicePicture { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class RegisterServiceCommandHandler : IRequestHandler<RegisterServiceCommand, RegisterServiceCommandResponse>
    {
        private readonly AppDbContext _context;

        public RegisterServiceCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegisterServiceCommandResponse> Handle(RegisterServiceCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Services.AnyAsync(s => s.Name == request.Name, cancellationToken))
                throw new Exception("Serviço com este nome já cadastrado");

            var service = new ServiceModel(request.Name, request.Description, request.ServicePicture);

            _context.Services.Add(service);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterServiceCommandResponse
            {
                ServiceId = service.Id.ToString(),
                Name = service.Name,
                Description = service.Description,
                ServicePicture = service.ServicePicture,
                Message = "Serviço registrado com sucesso"
            };
        }
    }
}
