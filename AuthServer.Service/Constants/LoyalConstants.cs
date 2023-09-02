using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service.Constants
{
    public static class LoyalConstants
    {
        public static string DiscountCoupon(Guid userId) => $"Discount{userId}";
    }
}
