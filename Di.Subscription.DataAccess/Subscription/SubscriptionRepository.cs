using System;
using System.Collections.Generic;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Subscription
{

    internal class SubscriptionRepository : ISubscriptionRepository
    {

        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public SubscriptionRepository(ISubscriptionDataAccess subscriptionDataAccess, IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<Subscription> GetSubscriptions(long customerNumber, string source, DateTime limitDate)
        {
            var subscriptionsDataSet = _subscriptionDataAccess.GetSubscriptions(customerNumber, source, limitDate, string.Empty, string.Empty);

            return _objectConverter.ConvertFromDataSet<Subscription>(subscriptionsDataSet);
        }
    }
}