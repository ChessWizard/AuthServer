using AuthServer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    public class OrderDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public OrderStatus OrderStatus { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset? ModifiedDate { get; set; }

        public AddressDto Address { get; set; }
    }
}
