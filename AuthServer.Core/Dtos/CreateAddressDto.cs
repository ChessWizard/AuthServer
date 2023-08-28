using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    public class CreateAddressDto
    {
        public Guid NeighborhoodId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
        
    }
}
