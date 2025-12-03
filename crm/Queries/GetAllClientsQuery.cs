using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;

namespace Queries
{
    public class GetAllClientsQuery : IRequest<GetAllClientsQueryResponse>
    {
    }

    public class GetAllClientsQueryResponse
    {
        public List<ClientResponseDto> Clients { get; set; } = new List<ClientResponseDto>();
    }

    public class ClientResponseDto
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

    public class GetAllClientsQueryHandler : IRequestHandler<GetAllClientsQuery, GetAllClientsQueryResponse>
    {
        private readonly AppDbContext _context;

        public GetAllClientsQueryHandler(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GetAllClientsQueryResponse> Handle(GetAllClientsQuery request, CancellationToken cancellationToken)
        {
            var clients = await _context.Clients
                .Include(c => c.Company)
                .Select(c => new ClientResponseDto
                {
                    Id = c.Id.ToString(),
                    Name = c.Name,
                    Email = c.Email,
                    Phone = c.Phone,
                    CompanyId = c.CompanyId.ToString(),
                    CompanyName = c.Company.Name,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return new GetAllClientsQueryResponse
            {
                Clients = clients
            };
        }
    }
}
