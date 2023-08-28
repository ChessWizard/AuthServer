using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    public class TownDto
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public CityDto City { get; set; }
    }
}
