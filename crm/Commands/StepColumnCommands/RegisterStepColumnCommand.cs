using MediatR;
using Models;
using Data;
using System.ComponentModel.DataAnnotations;

namespace Commands.StepColumnCommands
{
    public class RegisterStepColumnCommand : IRequest<RegisterStepColumnCommandResponse>
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Ordem é obrigatória")]
        public int Order { get; set; }

        [Required(ErrorMessage = "Cor é obrigatória")]
        public string Color { get; set; }
    }

    public class RegisterStepColumnCommandResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public string Color { get; set; }
        public bool IsActive { get; set; }
        public string? Message { get; set; }
    }

    public class RegisterStepColumnCommandHandler : IRequestHandler<RegisterStepColumnCommand, RegisterStepColumnCommandResponse>
    {
        private readonly AppDbContext _context;

        public RegisterStepColumnCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<RegisterStepColumnCommandResponse> Handle(RegisterStepColumnCommand request, CancellationToken cancellationToken)
        {
            var stepColumn = new StepColumnModel(request.Name, request.Order, request.Color);
            _context.StepColumn.Add(stepColumn);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterStepColumnCommandResponse
            {
                Id = stepColumn.Id,
                Name = stepColumn.Name,
                Order = stepColumn.Order,
                Color = stepColumn.Color,
                IsActive = stepColumn.IsActive,
                Message = "Coluna cadastrada com sucesso"
            };
        }
    }
}
