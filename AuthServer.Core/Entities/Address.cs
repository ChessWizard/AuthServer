using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class Address : AuditEntity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid UserAppId { get; set; }

        public UserApp UserApp { get; set; }

        public Guid NeighborhoodId { get; set; }

        public Neighborhood Neighborhood { get; set; }
    }
}
