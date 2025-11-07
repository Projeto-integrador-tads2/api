using Microsoft.AspNetCore.Mvc;
using MediatR;
using Commands.CompanyCardCommands;
using Queries;
using Dtos.CompanyCardDtos;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyCardController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyCardController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CompanyCardDto dto)
        {
            var command = new RegisterCompanyCardCommand
            {
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                StepColumnId = dto.StepColumnId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("update/{companyCardId}")]
        public async Task<IActionResult> Update(Guid companyCardId, [FromBody] CompanyCardDto dto)
        {
            var command = new UpdateCompanyCardCommand
            {
                CompanyCardId = companyCardId,
                UserId = dto.UserId,
                CompanyId = dto.CompanyId,
                StepColumnId = dto.StepColumnId
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete/{companyCardId}")]
        public async Task<IActionResult> Delete(Guid companyCardId)
        {
            var command = new DeleteCompanyCardCommand { CompanyCardId = companyCardId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{companyCardId}")]
        public async Task<IActionResult> GetById(Guid companyCardId)
        {
            var query = new GetCompanyCardByIdQuery { CompanyCardId = companyCardId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCompanyCardsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("cards/{columnId}")]
        public async Task<IActionResult> GetCardsByColumnId(Guid columnID)
        {
            var query = new GetCompanyCardsByColumnIdQuery { ColumnId = columnID };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
            [HttpGet("cards/grouped")]
            public async Task<IActionResult> GetCardsGroupedByColumn()
            {
                var query = new GetAllCompanyCardsGroupedByColumnQuery();
                var result = await _mediator.Send(query);
                return Ok(result);
            }
    }
}
