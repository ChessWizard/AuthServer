using AuthServer.Core.Entities.Common;
using AuthServer.Core.Entities.Common.Interfaces;
using AuthServer.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class UserApp : IdentityUser<Guid>, ITraceableEntity, ISoftDeleteEntity
    {
        public UserState UserState { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public ICollection<UserRefreshToken> UserRefreshToken { get; set; }

        public ICollection<Product> Products { get; set; }

        public ICollection<Address> Addresses { get; set; }
        
        public bool IsDeleted { get; set; }

        public bool IsActive { get; set; }
    }
}
