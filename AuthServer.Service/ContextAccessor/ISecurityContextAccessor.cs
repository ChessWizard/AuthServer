using Microsoft.AspNetCore.Http;

namespace AuthServer.Data.ContextAccessor
{
    // Http isteği geldikten sonra authentication bilgilerini yakalamamıza yarayacak interface
    public interface ISecurityContextAccessor
    {
        // Veriler yalnızca get edilsin, içeride değiştirilemesinler
        public Guid TokenId { get; }

        public Guid UserId { get; }

        public string Email { get; }

        public string Username { get; }

        public bool? IsAuthenticated { get; }
    }
}
