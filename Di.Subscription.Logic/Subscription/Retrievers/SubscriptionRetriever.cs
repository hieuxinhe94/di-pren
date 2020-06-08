using System;
using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.Subscription;

namespace Di.Subscription.Logic.Subscription.Retrievers
{
    internal class SubscriptionRetriever : ISubscriptionRetriever
    {
        private readonly ISubscriptionRepository _subscriptionRepository;

        public SubscriptionRetriever(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }

        public IEnumerable<Types.Subscription> GetSubscriptions(long customerNumber, string source, DateTime limitDate)
        {
           return _subscriptionRepository.GetSubscriptions(customerNumber, source, limitDate).Select(GetSubscription);
        }

        private Types.Subscription GetSubscription(DataAccess.Subscription.Subscription subscription)
        {
            return new Types.Subscription
            {
                SubscriptionNumber = subscription.OrderSubscriptionNumber,
                StartDate = subscription.SubscriptionStartDate,
                EndDate = subscription.SubscriptionEndDate,
                PriceGroup = subscription.PriceGroup,
                SubscriptionState = subscription.SubscriptionState,
                PackageId = subscription.PackageId,
                SubscriptionKind = subscription.SubscriptionKind,
                ProductNumber = subscription.ProductNumber,
                PaperCode = subscription.PaperCode,
                ExternalNumber = subscription.OrderExtNumber
            };
        } 
    }
}