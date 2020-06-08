using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Reclaim
{
    public interface IReclaimRepository
    {
        IEnumerable<ReclaimType> GetReclaimTypes(string paperCode);

        IEnumerable<Reclaim> GetCustomerReclaims(long customerNumber);

        string CreateDeliveryReclaim(
            long customerNumber,
            long subscriptionNumber,
            int extNo, //todo: change to more descriptive variable name
            string paperCode,
            int reclaimItem,
            string reclaimChannel,
            string reclaimKind,
            DateTime publishDate,
            bool creditSubscriber,
            bool reclaimMessage,
            DateTime deliveryMessageDate,
            string reclaimCode,
            string reclaimText,
            string responsiblePerson,
            string language,
            string reclaimPaper,
            string userId,
            string doorCode);
    }
}
