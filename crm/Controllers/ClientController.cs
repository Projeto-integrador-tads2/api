using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Commands;
using Dtos;
using Attributes;
using Enums;
using Queries;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ClientController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registra um novo cliente
        /// </summary>
        [HttpPost("register")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> Register([FromBody] Dtos.ClientDto dto)
        {
            var command = new RegisterClientCommand
            {
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone,
                CompanyId = dto.CompanyId
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Atualiza um cliente existente
        /// </summary>
        [HttpPut("update/{id}")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> Update(string id, [FromBody] Dtos.ClientDto dto)
        {
            var command = new UpdateClientCommand
            {
                ClientId = id,
                Name = dto.Name,
                Email = dto.Email,
                Phone = dto.Phone
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Deleta um cliente
        /// </summary>
        [HttpDelete("delete/{id}")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            var command = new DeleteClientCommand
            {
                ClientId = id
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Lista todos os clientes
        /// </summary>
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllClientsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Busca um cliente por ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetClientByIdQuery
            {
                ClientId = id
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
