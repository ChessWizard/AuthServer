using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    public class UserAppDto
    {
        public Guid Id { get; set; }
        
        public string UserName { get; set; }

        public string Email { get; set; }

        public List<AddressDto> Addresses { get; set; }
    }
}
