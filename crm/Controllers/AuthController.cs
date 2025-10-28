using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Commands;
using Dtos;
using Attributes;
using Enums;

namespace Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Registra um novo usuário
        /// </summary>
        [HttpPost("register")]
        [Authorize]
        [RequireRole(UserRole.Admin)]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            var command = new RegisterUserCommand
            {
                Name = dto.Name,
                Email = dto.Email,
                Password = dto.Password,
                Phone = dto.Phone
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Realiza login do usuário
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var command = new LoginUserCommand
            {
                Email = dto.Email,
                Password = dto.Password
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Envia link para redefinição de senha
        /// </summary>
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordDto dto)
        {
            var command = new ForgotPasswordCommand
            {
                Email = dto.Email
            };

            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// DESENVOLVIMENTO: Cria usuário de teste (REMOVER EM PRODUÇÃO!)
        /// </summary>
        [HttpPost("create-test-user")]
        public async Task<IActionResult> CreateTestUser()
        {
            var command = new RegisterUserCommand
            {
                Name = "Gabriel",
                Email = "gabriel@gmail.com",
                Password = "gabriel@123",
                Phone = "15999673508"
            };

            try
            {
                var result = await _mediator.Send(command);
                return Ok(new
                {
                    message = "Usuário de teste criado com sucesso!",
                    userId = result.UserId,
                    email = result.Email,
                    testCredentials = new
                    {
                        email = "gabriel@gmail.com",
                        password = "gabriel@123"
                    }
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}
