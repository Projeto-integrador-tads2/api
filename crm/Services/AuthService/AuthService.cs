using Data;
using Dtos;
using Models;
using Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Alure.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly PasswordHasher<UserModel> _passwordHasher;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AuthService(AppDbContext context, IConfiguration configuration, IEmailService emailService)
        {
            _context = context;
            _passwordHasher = new PasswordHasher<UserModel>();
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<UserModel> RegisterAsync(RegisterDto userDto)
        {
            if (await _context.User.AnyAsync(u => u.Email == userDto.Email))
                throw new Exception("Email já cadastrado");

            var user = new UserModel
            {
                Name = userDto.Name,
                Email = userDto.Email,
                Phone = userDto.Phone,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            user.Password = _passwordHasher.HashPassword(user, userDto.Password);

            _context.User.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || 
                _passwordHasher.VerifyHashedPassword(user, user.Password, loginDto.Password) == PasswordVerificationResult.Failed)
                throw new Exception("Usuário ou senha inválidos");

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return new AuthResponseDto
            {
                Token = tokenString,
                UserId = user.Id,
                Name = user.Name,
                Email = user.Email
            };
        }

        //Arrumar a parte de envio de email
        public async Task SendLinkPasswordRedefinition(string email)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return;

            //Adicionar o Jwt no Program.cs
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

            //Revisar essa parte
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            var link = $"http://localhost:3000/RedefinirSenha?token={Uri.EscapeDataString(tokenString)}";

            await _emailService.SendEmailAsync(
                user.Email,
                user.Name,
                "Redefina sua senha",
                $"Clique no botão abaixo para redefinir sua senha:",
                "Redefinir senha",
                link
            );
        }
    }
}
