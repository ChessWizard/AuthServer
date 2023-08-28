using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class UserRefreshToken : BaseEntity<Guid>
    {
        public string Code { get; set; }

        public DateTime Expiration { get; set; }

        public Guid UserAppId { get; set; }

        public UserApp UserApp { get; set; }
    }
}
