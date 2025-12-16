using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using Dtos.ServiceDtos;

namespace Commands.ServiceCommands
{
    public class RegisterServiceCommand : IRequest<RegisterServiceCommandResponse>
    {
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

    public class RegisterServiceCommandResponse
    {
        public Guid ServiceId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int ContractDuration { get; set; }
        public decimal Value { get; set; }
        public string? ServicePicture { get; set; }
        public string? Message { get; set; }
    }

    public class RegisterServiceCommandHandler : IRequestHandler<RegisterServiceCommand, RegisterServiceCommandResponse>
    {
        private readonly Data.AppDbContext _context;
        public RegisterServiceCommandHandler(Data.AppDbContext context) { _context = context; }
        public async Task<RegisterServiceCommandResponse> Handle(RegisterServiceCommand request, CancellationToken cancellationToken)
        {
            var service = new ServiceModel(request.Name, request.Description, request.ContractDuration, request.Value, request.ServicePicture);
            _context.Services.Add(service);
            await _context.SaveChangesAsync(cancellationToken);
            return new RegisterServiceCommandResponse
            {
                ServiceId = service.Id,
                Name = service.Name,
                Description = service.Description,
                ContractDuration = service.ContractDuration,
                Value = service.Value,
                ServicePicture = service.ServicePicture,
                Message = "Serviço cadastrado com sucesso"
            };
        }
    }
}