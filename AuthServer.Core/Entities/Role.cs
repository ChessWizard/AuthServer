using AuthServer.Core.Enums;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public RoleType RoleType { get; set; }

        public string Title { get; set; }
    }
}
