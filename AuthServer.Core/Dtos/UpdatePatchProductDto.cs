using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    public class UpdatePatchProductDto
    {
        public string Name { get; set; }

        public decimal? Price { get; set; }

        public int? Stock { get; set; }

        public bool? IsDeleted { get; set; }
    }
}
