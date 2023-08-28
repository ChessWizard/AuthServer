using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class Town : BaseEntity<Guid>
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public Guid CityId { get; set; }

        public City City { get; set; }

        public ICollection<Neighborhood> Neighborhoods { get; set; }
    }
}
