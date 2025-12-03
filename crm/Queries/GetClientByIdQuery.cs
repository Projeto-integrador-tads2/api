using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Queries
{
    public class GetClientByIdQuery : IRequest<GetClientByIdQueryResponse>
    {
        [Required(ErrorMessage = "Id é obrigatório")]
        public string ClientId { get; set; } = string.Empty;
    }

    public class GetClientByIdQueryResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string CompanyId { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class GetClientByIdQueryHandler : IRequestHandler<GetClientByIdQuery, GetClientByIdQueryResponse>
    {
        private readonly AppDbContext _context;

        public GetClientByIdQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetClientByIdQueryResponse> Handle(GetClientByIdQuery request, CancellationToken cancellationToken)
        {
            var clientId = Guid.Parse(request.ClientId);
            var client = await _context.Clients
                .Include(c => c.Company)
                .FirstOrDefaultAsync(c => c.Id == clientId, cancellationToken);

            if (client == null)
                throw new Exception("Cliente não encontrado");

            return new GetClientByIdQueryResponse
            {
                Id = client.Id.ToString(),
                Name = client.Name,
                Email = client.Email,
                Phone = client.Phone,
                CompanyId = client.CompanyId.ToString(),
                CompanyName = client.Company.Name,
                CreatedAt = client.CreatedAt,
                UpdatedAt = client.UpdatedAt
            };
        }
    }
}
