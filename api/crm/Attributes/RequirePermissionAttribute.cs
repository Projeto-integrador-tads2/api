using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Enums;
using System.Security.Claims;
using Data;
using Models;
using Microsoft.EntityFrameworkCore;

namespace Attributes
{
    public class RequirePermissionAttribute : TypeFilterAttribute
    {
        public RequirePermissionAttribute(Resource resource, Permission permission)
            : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { resource, permission };
        }

        private class PermissionFilter : IAsyncAuthorizationFilter
        {
            private readonly Resource _resource;
            private readonly Permission _permission;
            private readonly AppDbContext _context;

            public PermissionFilter(Resource resource, Permission permission, AppDbContext context)
            {
                _resource = resource;
                _permission = permission;
                _context = context;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Usuário não autenticado" });
                    return;
                }

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "ID de usuário inválido" });
                    return;
                }

                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Usuário não encontrado" });
                    return;
                }

                if (!user.HasPermission(_resource, _permission))
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }

    public class RequireRoleAttribute : TypeFilterAttribute
    {
        public RequireRoleAttribute(UserRole role)
            : base(typeof(RoleFilter))
        {
            Arguments = new object[] { role };
        }

        private class RoleFilter : IAsyncAuthorizationFilter
        {
            private readonly UserRole _requiredRole;
            private readonly AppDbContext _context;

            public RoleFilter(UserRole requiredRole, AppDbContext context)
            {
                _requiredRole = requiredRole;
                _context = context;
            }

            public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
            {
                var userIdClaim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Usuário não autenticado" });
                    return;
                }

                if (!Guid.TryParse(userIdClaim.Value, out var userId))
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "ID de usuário inválido" });
                    return;
                }

                var user = await _context.User.FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { message = "Usuário não encontrado" });
                    return;
                }

                if (user.Role != _requiredRole)
                {
                    context.Result = new ForbidResult();
                    return;
                }
            }
        }
    }
}
