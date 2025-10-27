using MediatR;
using Data;
using Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Commands
{
    public class RegisterUserCommand : IRequest<RegisterUserCommandResponse>
    {
        [Required(ErrorMessage = "Nome é obrigatório")]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Senha é obrigatória")]
        [MinLength(6, ErrorMessage = "Senha deve ter pelo menos 6 caracteres")]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Telefone é obrigatório")]
        public string Phone { get; set; } = string.Empty;
    }

    public class RegisterUserCommandResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }

    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, RegisterUserCommandResponse>
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<object> _passwordHasher;

        public RegisterUserCommandHandler(AppDbContext context)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<object>();
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            if (await _context.User.AnyAsync(u => u.Email == request.Email, cancellationToken))
                throw new Exception("Email já cadastrado");

            var tempUser = new { Name = request.Name, Email = request.Email };
            var hashedPassword = _passwordHasher.HashPassword(tempUser, request.Password);

            var user = new UserModel(request.Name, request.Email, hashedPassword, request.Phone);

            _context.User.Add(user);
            await _context.SaveChangesAsync(cancellationToken);

            return new RegisterUserCommandResponse
            {
                UserId = user.Id.ToString(),
                Name = user.Name,
                Email = user.Email,
                Message = "Usuário registrado com sucesso"
            };
        }
    }
}
