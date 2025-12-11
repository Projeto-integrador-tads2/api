using Microsoft.AspNetCore.Mvc;
using MediatR;
using Dtos.ServiceDtos;
using Commands.ServiceCommands;
using Queries.ServiceQueries;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ServiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterServiceDto dto)
        {
            var command = new Commands.ServiceCommands.RegisterServiceCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                ContractDuration = dto.ContractDuration,
                Value = dto.Value,
                ServicePicture = dto.ServicePicture
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("update/{serviceId}")]
        public async Task<IActionResult> Update(Guid serviceId, [FromBody] UpdateServiceDto dto)
        {
            var command = new Commands.ServiceCommands.UpdateServiceCommand
            {
                ServiceId = serviceId,
                Name = dto.Name,
                Description = dto.Description,
                ContractDuration = dto.ContractDuration,
                Value = dto.Value,
                ServicePicture = dto.ServicePicture
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete/{serviceId}")]
        public async Task<IActionResult> Delete(Guid serviceId)
        {
            var command = new Commands.ServiceCommands.DeleteServiceCommand { ServiceId = serviceId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{serviceId}")]
        public async Task<IActionResult> GetById(Guid serviceId)
        {
            var query = new Queries.ServiceQueries.GetServiceByIdQuery { ServiceId = serviceId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new Queries.ServiceQueries.GetAllServicesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}