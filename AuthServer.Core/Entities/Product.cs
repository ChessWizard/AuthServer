using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class Product : AuditEntity<Guid>
    {
        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public string UserAppId { get; set; }

        public UserApp UserApp { get; set; }
    }
}
