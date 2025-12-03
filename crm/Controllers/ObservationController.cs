using Microsoft.AspNetCore.Mvc;
using MediatR;
using Commands;
using Queries;
using Dtos;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ObservationController : ControllerBase
    {
    private readonly IMediator _mediator;
    private readonly Interfaces.ICurrentUserService _currentUserService;

        public ObservationController(IMediator mediator, Interfaces.ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterObservationDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId();
            if (userId == null)
                return Unauthorized("Usuário não autenticado.");
            var command = new RegisterObservationCommand
            {
                Title = dto.Title,
                Content = dto.Content,
                UserId = userId.Value,
                CompanyCardId = dto.CompanyCardId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("update/{observationId}")]
        public async Task<IActionResult> Update(Guid observationId, [FromBody] UpdateObservationDto dto)
        {
            var command = new UpdateObservationCommand
            {
                ObservationId = observationId,
                Title = dto.Title,
                Content = dto.Content
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete/{observationId}")]
        public async Task<IActionResult> Delete(Guid observationId)
        {
            var command = new DeleteObservationCommand { ObservationId = observationId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{observationId}")]
        public async Task<IActionResult> GetById(Guid observationId)
        {
            var query = new GetObservationByIdQuery { ObservationId = observationId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] Guid? companyCardId)
        {
            var query = new GetAllObservationsQuery { CompanyCardId = companyCardId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}