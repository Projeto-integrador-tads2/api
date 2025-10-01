using Dtos;
using Models;

namespace Services
{
    public interface IAuthService
    {
        Task<UserModel> RegisterAsync(RegisterDto usuarioDto);
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task SendLinkPasswordRedefinition(string email);
    }
}
