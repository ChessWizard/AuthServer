using AuthServer.Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace AuthServer.API.Common
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class AuthServerAuthorizeAttribute : AuthorizeAttribute
    {
        public AuthServerAuthorizeAttribute(params RoleType[] roles)
        {
            Roles = string.Join(",", roles.Select(role => role.ToString()));
        }
    }
}
