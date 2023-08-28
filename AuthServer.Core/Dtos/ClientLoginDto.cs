using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Dtos
{
    // Bir client'ın authentice olması için gerekli bilgiler
    public class ClientLoginDto
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }
    }
}
