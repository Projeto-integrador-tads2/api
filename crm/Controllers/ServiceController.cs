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
    public class ServiceController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly Interfaces.ICurrentUserService _currentUserService;

        public ServiceController(IMediator mediator, Interfaces.ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Registra um novo serviço com foto (opcional)
        /// </summary>
        /// <remarks>
        /// Este endpoint permite registrar um serviço e fazer upload da foto em uma única requisição.
        /// A foto é opcional. O usuário logado (via JWT) será usado automaticamente.
        /// </remarks>
        [HttpPost("register")]
        [Consumes("multipart/form-data")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> RegisterWithPicture([FromForm] ServiceDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId();
            var userName = _currentUserService.GetCurrentUserName();
            var userEmail = _currentUserService.GetCurrentUserEmail();

            if (userId == null)
                return Unauthorized(new { message = "Usuário não autenticado" });

            var command = new RegisterServiceWithPictureCommand
            {
                Name = dto.Name,
                Description = dto.Description,
                Picture = dto.Picture,
                CustomFileName = dto.CustomFileName
            };

            var result = await _mediator.Send(command);

            return Ok(new
            {
                result.ServiceId,
                result.Name,
                result.Description,
                result.ServicePicture,
                result.Message,
                CreatedBy = new
                {
                    UserId = userId.ToString(),
                    UserName = userName,
                    UserEmail = userEmail
                }
            });
        }

        /// <summary>
        /// Atualiza um serviço existente com foto (opcional)
        /// </summary>
        /// <remarks>
        /// Este endpoint permite atualizar um serviço e trocar/adicionar a foto.
        /// - Para adicionar/trocar foto: envie o arquivo no campo 'picture'
        /// - Para manter a foto atual: não envie o campo 'picture'
        /// </remarks>
        [HttpPut("update/{id}")]
        [Consumes("multipart/form-data")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> UpdateWithPicture(
            string id,
            [FromForm] ServiceDto dto)
        {
            var userId = _currentUserService.GetCurrentUserId();
            var userName = _currentUserService.GetCurrentUserName();

            if (userId == null)
                return Unauthorized(new { message = "Usuário não autenticado" });

            var command = new UpdateServiceWithPictureCommand
            {
                ServiceId = id,
                Name = dto.Name,
                Description = dto.Description,
                Picture = dto.Picture,
                CustomFileName = dto.CustomFileName
            };

            var result = await _mediator.Send(command);

            return Ok(new
            {
                result.ServiceId,
                result.Name,
                result.Description,
                result.ServicePicture,
                result.Message,
                UpdatedBy = new
                {
                    UserId = userId.ToString(),
                    UserName = userName
                }
            });
        }

        /// <summary>
        /// Deleta um serviço
        /// </summary>
        [HttpDelete("delete/{id}")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = _currentUserService.GetCurrentUserId();

            if (userId == null)
                return Unauthorized(new { message = "Usuário não autenticado" });

            var command = new DeleteServiceCommand
            {
                ServiceId = id
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Lista todos os serviços
        /// </summary>
        [HttpGet("all")]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetAllServicesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Busca um serviço por ID
        /// </summary>
        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetById(string id)
        {
            var query = new GetServiceByIdQuery
            {
                ServiceId = id
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Endpoint separado para upload de foto de serviço existente
        /// </summary>
        /// <remarks>
        /// Use este endpoint apenas se quiser fazer upload da foto separadamente.
        /// Para registrar/atualizar com foto, use os endpoints /register ou /update
        /// </remarks>
        [HttpPost("{serviceId}/picture")]
        [Consumes("multipart/form-data")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> UploadServicePicture(
            Guid serviceId,
            [FromForm] FileUploadDto request)
        {
            var userId = _currentUserService.GetCurrentUserId();

            if (userId == null)
                return Unauthorized(new { message = "Usuário não autenticado" });

            var command = new UploadServicePictureCommand
            {
                ServiceId = serviceId,
                File = request.File!,
                CustomFileName = request.CustomFileName
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Remove apenas a foto de um serviço existente
        /// </summary>
        /// <remarks>
        /// Este endpoint remove a foto do serviço mas mantém o serviço cadastrado.
        /// O serviço continuará existindo, apenas sem foto.
        /// </remarks>
        [HttpDelete("{id}/picture")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> RemoveServicePicture(string id)
        {
            var userId = _currentUserService.GetCurrentUserId();
            var userName = _currentUserService.GetCurrentUserName();

            if (userId == null)
                return Unauthorized(new { message = "Usuário não autenticado" });

            var command = new RemoveServicePictureCommand
            {
                ServiceId = id
            };

            var result = await _mediator.Send(command);

            return Ok(new
            {
                result.ServiceId,
                result.Name,
                result.Description,
                ServicePicture = (string?)null,
                result.Message,
                UpdatedBy = new
                {
                    UserId = userId.ToString(),
                    UserName = userName
                }
            });
        }
    }
}
