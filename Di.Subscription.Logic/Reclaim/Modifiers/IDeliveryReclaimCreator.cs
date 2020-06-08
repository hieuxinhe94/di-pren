using System;

namespace Di.Subscription.Logic.Reclaim.Modifiers
{
    internal interface IDeliveryReclaimCreator
    {
        string CreateDeliveryReclaim(
            long customerNumber,
            long subscriptionNumber,
            int extNo,
            string paperCode,
            string reasonId,
            DateTime date);
    }
}
