using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace AuthServer.Data.ContextAccessor
{
    public class SecurityContextAccessor : ISecurityContextAccessor
    {
        private readonly IHttpContextAccessor _contextAccessor;

        public SecurityContextAccessor(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid TokenId => Guid.Parse(_contextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Jti));

        public Guid UserId => Guid.Parse(_contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier));

        public string Email => _contextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);

        public string Username => _contextAccessor.HttpContext?.User?.Identity?.Name;
        
        public bool? IsAuthenticated => _contextAccessor?.HttpContext?.User?.Identity?.IsAuthenticated;
    }
}
