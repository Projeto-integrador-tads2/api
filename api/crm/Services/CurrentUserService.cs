using Interfaces;
using Models;
using Data;
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
