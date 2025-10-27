using MediatR;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class ForgotPasswordCommand : IRequest<ForgotPasswordCommandResponse>
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
    }

    public class ForgotPasswordCommandResponse
    {
        public string Message { get; set; } = string.Empty;
        public bool Success { get; set; }
    }

    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, ForgotPasswordCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public ForgotPasswordCommandHandler(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ForgotPasswordCommandResponse> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);
                
                if (user == null)
                {
                    return new ForgotPasswordCommandResponse
                    {
                        Message = "Se o email existir em nossa base, um link de redefinição será enviado",
                        Success = true
                    };
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? "");

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim("purpose", "reset-password")
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(15), 
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                var resetLink = $"http://localhost:3000/reset-password?token={Uri.EscapeDataString(tokenString)}";

                // TODO: Implementar envio de email
                // Simulando envio de email por enquanto
                Console.WriteLine($"Email enviado para {user.Email} com link: {resetLink}");

                return new ForgotPasswordCommandResponse
                {
                    Message = "Se o email existir em nossa base, um link de redefinição será enviado",
                    Success = true
                };
            }
            catch (Exception)
            {
                return new ForgotPasswordCommandResponse
                {
                    Message = "Se o email existir em nossa base, um link de redefinição será enviado",
                    Success = true
                };
            }
        }
    }
}
