using Interfaces;
using Models;
using Data;
using Enums;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }

        public Guid? GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return null;
            }

            return userId;
        }

        public string? GetCurrentUserName()
        {
            var nameClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
            return nameClaim?.Value;
        }

        public string? GetCurrentUserEmail()
        {
            var emailClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);
            return emailClaim?.Value;
        }

        public UserRole? GetCurrentUserRole()
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role);

            if (roleClaim == null || !Enum.TryParse<UserRole>(roleClaim.Value, out var role))
            {
                return null;
            }

            return role;
        }

        public async Task<UserModel?> GetCurrentUserAsync()
        {
            var userId = GetCurrentUserId();

            if (userId == null)
            {
                return null;
            }

            return await _context.User.FirstOrDefaultAsync(u => u.Id == userId.Value);
        }
    }
}
