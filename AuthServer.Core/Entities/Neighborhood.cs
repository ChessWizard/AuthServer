using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class Neighborhood : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid TownId { get; set; }

        public Town Town { get; set; }

        public ICollection<Address> Addresses { get; set; }
    }
}
