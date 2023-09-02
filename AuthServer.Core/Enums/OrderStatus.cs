using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Core.Enums
{
    public enum OrderStatus
    {
        WaitingForApprove = 1,
        Approved = 2,
        Sent = 3,
        Delivered = 4
    }
}
