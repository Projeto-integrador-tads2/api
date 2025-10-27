using Models;

namespace Interfaces
{
    public interface ICurrentUserService
    {
        Task<UserModel?> GetCurrentUserAsync();
        Guid? GetCurrentUserId();
    }
}
