using AuthServer.Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    public class CreateOrderDto
    {
        public List<RequestedProduct> RequestedProducts { get; set; }
    }

    public class RequestedProduct
    {
        public Guid ProductId { get; set; }

        public int Count { get; set; } = 1;
    }
}
