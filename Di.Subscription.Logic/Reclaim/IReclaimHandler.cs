using System;
using System.Collections.Generic;

namespace Di.Subscription.Logic.Reclaim
{
    public interface IReclaimHandler
    {
        string CreateDeliveryReclaim(
            long customerNumber, 
            long subscriptionNumber, 
            int extNo, 
            string paperCode,
            string reasonId, 
            DateTime date);

        IEnumerable<Types.ReclaimType> GetReclaimTypes();

        IEnumerable<Types.Reclaim> GetCustomerReclaims(long customerNumber);
    }
}
