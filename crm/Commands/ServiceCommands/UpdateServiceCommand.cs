using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Dtos.ServiceDtos;

namespace Commands.ServiceCommands
{
    public class UpdateServiceCommand : IRequest<UpdateServiceCommandResponse>
    {
        [Required]
        public Guid ServiceId { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public int ContractDuration { get; set; }
        [Required]
        public decimal Value { get; set; }
        public string? ServicePicture { get; set; }
    }

    public class UpdateServiceCommandResponse
    {
        public Guid ServiceId { get; set; }
        public string? Message { get; set; }
    }

    public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, UpdateServiceCommandResponse>
    {
        private readonly Data.AppDbContext _context;
        public UpdateServiceCommandHandler(Data.AppDbContext context) { _context = context; }
        public async Task<UpdateServiceCommandResponse> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
        {
            var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == request.ServiceId, cancellationToken);
            if (service == null)
                throw new Exception("Serviço não encontrado");
            service.Update(request.Name, request.Description, request.ContractDuration, request.Value, request.ServicePicture);
            await _context.SaveChangesAsync(cancellationToken);
            return new UpdateServiceCommandResponse
            {
                ServiceId = service.Id,
                Message = "Serviço atualizado com sucesso"
            };
        }
    }
}