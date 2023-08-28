using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class City : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<Town> Towns { get; set; }
    }
}
