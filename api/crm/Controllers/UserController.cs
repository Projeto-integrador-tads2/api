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

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Realiza upload de imagem que será usada como foto de perfil do usuário
        /// </summary>
        [HttpPost("{userId}/profile-picture")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProfilePicture(
            Guid userId, 
            [FromForm] FileUploadDto request)
        {
                var command = new UploadUserProfilePictureCommand
                {
                    UserId = userId,
                    File = request.File!,
                    CustomFileName = request.CustomFileName
                };

                var result = await _mediator.Send(command);
                return Ok(result);
        }
    }
}