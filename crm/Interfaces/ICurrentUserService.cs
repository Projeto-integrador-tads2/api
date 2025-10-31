using Models;
using Enums;

namespace Interfaces
{
    public interface ICurrentUserService
    {
        Task<UserModel?> GetCurrentUserAsync();
        Guid? GetCurrentUserId();
        string? GetCurrentUserName();
        string? GetCurrentUserEmail();
        UserRole? GetCurrentUserRole();
    }
}
