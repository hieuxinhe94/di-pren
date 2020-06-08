using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Reclaim.Modifiers;
using Di.Subscription.Logic.Reclaim.Retrievers;

namespace Di.Subscription.Logic.Reclaim
{
    /// <summary>
    /// This is a facade for the reclaim handling which clients should use, no logic in this class
    /// </summary>
    internal class ReclaimHandler : IReclaimHandler
    {
        private readonly IDeliveryReclaimCreator _deliveryReclaimCreator;
        private readonly IReclaimTypeRetriever _reclaimTypeRetriever;
        private readonly IReclaimRetriever _reclaimRetriever;

        public ReclaimHandler(
            IDeliveryReclaimCreator deliveryReclaimCreator, 
            IReclaimTypeRetriever reclaimTypeRetriever,
            IReclaimRetriever reclaimRetriever)
        {
            _deliveryReclaimCreator = deliveryReclaimCreator;
            _reclaimTypeRetriever = reclaimTypeRetriever;
            _reclaimRetriever = reclaimRetriever;
        }

        public string CreateDeliveryReclaim(
            long customerNumber, 
            long subscriptionNumber, 
            int extNo, 
            string paperCode, 
            string reasonId,
            DateTime date)
        {
            return _deliveryReclaimCreator.CreateDeliveryReclaim(
                customerNumber,
                subscriptionNumber,
                extNo,
                paperCode,
                reasonId,
                date);
        }

        public IEnumerable<Types.ReclaimType> GetReclaimTypes()
        {
            return _reclaimTypeRetriever.GetReclaimTypes();
        }

        public IEnumerable<Types.Reclaim> GetCustomerReclaims(long customerNumber)
        {
            return _reclaimRetriever.GetCustomerReclaims(customerNumber);
        }
    }
}