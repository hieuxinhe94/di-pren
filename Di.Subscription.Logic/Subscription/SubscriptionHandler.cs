using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Subscription.Retrievers;

namespace Di.Subscription.Logic.Subscription
{
    internal class SubscriptionHandler : ISubscriptionHandler
    {
        private readonly ISubscriptionRetriever _subscriptionRetriever;

        public SubscriptionHandler(ISubscriptionRetriever subscriptionRetriever)
        {
            _subscriptionRetriever = subscriptionRetriever;
        }

        public IEnumerable<Types.Subscription> GetSubscriptions(long customerNumber, DateTime limitDate)
        {
            return _subscriptionRetriever.GetSubscriptions(customerNumber, SubscriptionConstants.Source, limitDate);
        }
    }

}
