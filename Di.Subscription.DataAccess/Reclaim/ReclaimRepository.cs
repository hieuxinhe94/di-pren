using System;
using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Reclaim
{
    internal class ReclaimRepository : IReclaimRepository 
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public ReclaimRepository(
            ISubscriptionDataAccess subscriptionDataAccess, 
            IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<ReclaimType> GetReclaimTypes(string paperCode)
        {
            var reclaimTypesDataSet = _subscriptionDataAccess.GetReclaimTypes(paperCode);
            return _objectConverter.ConvertFromDataSet<ReclaimType>(reclaimTypesDataSet);
        }

        public IEnumerable<Reclaim> GetCustomerReclaims(long customerNumber)
        {
            var reclaimsDataSet = _subscriptionDataAccess.GetCustomerReclaims(customerNumber);
            return _objectConverter.ConvertFromDataSet<Reclaim>(reclaimsDataSet);
        }


        public string CreateDeliveryReclaim(
            long customerNumber,
            long subscriptionNumber,
            int extNo,
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
            string doorCode)
        {
            return _subscriptionDataAccess.CreateDeliveryReclaim(
                customerNumber,
                subscriptionNumber,
                extNo,
                paperCode,
                reclaimItem,
                reclaimChannel,
                reclaimKind,
                publishDate,
                creditSubscriber,
                reclaimMessage,
                deliveryMessageDate,
                reclaimCode,
                reclaimText,
                responsiblePerson,
                language,
                reclaimPaper,
                userId,
                doorCode);
        }
    }
}
