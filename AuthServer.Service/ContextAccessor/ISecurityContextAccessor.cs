using Microsoft.AspNetCore.Http;

namespace AuthServer.Data.ContextAccessor
{
    public interface ISecurityContextAccessor
    {
        public Guid TokenId { get; }

        public Guid UserId { get; }

        public string Email { get; }

        public string Username { get; }

        public bool? IsAuthenticated { get; }
    }
}
