using Microsoft.AspNetCore.Mvc;
using MediatR;
using Commands.StepColumnCommands;
using Queries;
using Dtos.StepColumnDtos;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StepColumnController : ControllerBase
    {
        private readonly IMediator _mediator;

        public StepColumnController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] StepColumnDto dto)
        {
            var command = new RegisterStepColumnCommand
            {
                Name = dto.Name,
                Order = dto.Order,
                Color = dto.Color
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] StepColumnDto dto)
        {
            var command = new UpdateStepColumnCommand
            {
                Id = id,
                Name = dto.Name,
                Color = dto.Color
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var command = new DeleteStepColumnCommand { Id = id };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetStepColumnByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllStepColumnsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
