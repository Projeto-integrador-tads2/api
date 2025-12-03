using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Commands;
using Dtos;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
    private readonly IMediator _mediator;
    private readonly Interfaces.ICurrentUserService _currentUserService;

        public UserController(IMediator mediator, Interfaces.ICurrentUserService currentUserService)
        {
            _mediator = mediator;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Realiza upload de imagem que será usada como foto de perfil do usuário
        /// </summary>
        [HttpPost("profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture([FromForm] FileUploadDto request)
        {
            var userId = _currentUserService.GetCurrentUserId();
            if (userId == null)
                return Unauthorized("Usuário não autenticado.");
            var command = new UploadUserProfilePictureCommand
            {
                UserId = userId.Value,
                File = request.File!,
                CustomFileName = request.CustomFileName
            };
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}