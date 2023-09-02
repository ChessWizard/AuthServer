using AuthServer.Core.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class OrderProduct : BaseEntity<Guid>
    {
        public Guid? ProductId { get; set; }
        
        public Product? Product { get; set; }

        public Guid? OrderId { get; set; }

        public Order? Order { get; set; }
    }
}
