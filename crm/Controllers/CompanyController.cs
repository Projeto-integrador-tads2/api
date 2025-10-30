using Microsoft.AspNetCore.Mvc;
using MediatR;
using Commands;
using Dtos.CompanyDtos;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CompanyController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CompanyController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCompanyDto dto)
        {
            var command = new RegisterCompanyCommand
            {
                Name = dto.Name,
                Cnpj = dto.Cnpj,
                CompanyPicture = dto.CompanyPicture
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPut("update/{companyId}")]
        public async Task<IActionResult> Update(Guid companyId, [FromBody] UpdateCompanyDto dto)
        {
            var command = new UpdateCompanyCommand
            {
                CompanyId = companyId,
                Name = dto.Name,
                Cnpj = dto.Cnpj,
                CompanyPicture = dto.CompanyPicture
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpDelete("delete/{companyId}")]
        public async Task<IActionResult> Delete(Guid companyId)
        {
            var command = new DeleteCompanyCommand { CompanyId = companyId };
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("{companyId}")]
        public async Task<IActionResult> GetById(Guid companyId)
        {
            var query = new GetCompanyByIdQuery { CompanyId = companyId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllCompaniesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
