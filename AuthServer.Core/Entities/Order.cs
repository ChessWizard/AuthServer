using AuthServer.Core.Entities.Common;
using AuthServer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class Order : AuditEntity<Guid>, ISoftDeleteEntity
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public bool IsDeleted { get; set; }

        public Guid UserAppId { get; set; }

        public UserApp UserApp { get; set; }

        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
