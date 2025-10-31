using MediatR;
using Data;
using Models;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class UpdateServiceCommand : IRequest<UpdateServiceCommandResponse>
    {
        public string ServiceId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Descrição é obrigatória")]
        public string Description { get; set; } = string.Empty;

        public string? ServicePicture { get; set; }
    }

    public class UpdateServiceCommandResponse
    {
        public string ServiceId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? ServicePicture { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, UpdateServiceCommandResponse>
    {
        private readonly AppDbContext _context;

        public UpdateServiceCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UpdateServiceCommandResponse> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var serviceId = Guid.Parse(request.ServiceId);
            var service = await _context.Services.FindAsync(new object[] { serviceId }, cancellationToken);

            if (service == null)
                throw new Exception("Serviço não encontrado");

            service.Update(request.Name, request.Description, request.ServicePicture);
            await _context.SaveChangesAsync(cancellationToken);

            return new UpdateServiceCommandResponse
            {
                ServiceId = service.Id.ToString(),
                Name = service.Name,
                Description = service.Description,
                ServicePicture = service.ServicePicture,
                Message = "Serviço atualizado com sucesso"
            };
        }
    }
}
