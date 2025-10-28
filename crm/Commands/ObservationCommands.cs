using MediatR;
using Models;
using Data;
using Microsoft.EntityFrameworkCore;
using Dtos;

namespace Commands
{
    public class RegisterObservationCommand : IRequest<ObservationDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public Guid CompanyCardId { get; set; }
    }

    public class RegisterObservationCommandHandler : IRequestHandler<RegisterObservationCommand, ObservationDto>
    {
        private readonly AppDbContext _context;

        public RegisterObservationCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ObservationDto> Handle(RegisterObservationCommand request, CancellationToken cancellationToken)
        {
            var observation = new ObservationModel(request.Title, request.Content, request.UserId, request.CompanyCardId);
            _context.Observations.Add(observation);
            await _context.SaveChangesAsync(cancellationToken);

            return new ObservationDto
            {
                Id = observation.Id,
                Title = observation.Title,
                Content = observation.Content,
                UserId = observation.UserId,
                CompanyCardId = observation.CompanyCardId
            };
        }
    }

    public class UpdateObservationCommand : IRequest<ObservationDto>
    {
        public Guid ObservationId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateObservationCommandHandler : IRequestHandler<UpdateObservationCommand, ObservationDto>
    {
        private readonly AppDbContext _context;

        public UpdateObservationCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ObservationDto> Handle(UpdateObservationCommand request, CancellationToken cancellationToken)
        {
            var observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == request.ObservationId, cancellationToken);
            if (observation == null)
                throw new Exception("Observação não encontrada");

            observation.Update(request.Title, request.Content);
            await _context.SaveChangesAsync(cancellationToken);

            return new ObservationDto
            {
                Id = observation.Id,
                Title = observation.Title,
                Content = observation.Content,
                UserId = observation.UserId,
                CompanyCardId = observation.CompanyCardId
            };
        }
    }

    public class DeleteObservationCommand : IRequest<bool>
    {
        public Guid ObservationId { get; set; }
    }

    public class DeleteObservationCommandHandler : IRequestHandler<DeleteObservationCommand, bool>
    {
        private readonly AppDbContext _context;

        public DeleteObservationCommandHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteObservationCommand request, CancellationToken cancellationToken)
        {
            var observation = await _context.Observations.FirstOrDefaultAsync(o => o.Id == request.ObservationId, cancellationToken);
            if (observation == null)
                throw new Exception("Observação não encontrada");

            _context.Observations.Remove(observation);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
