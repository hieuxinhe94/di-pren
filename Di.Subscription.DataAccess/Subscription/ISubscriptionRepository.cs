using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Subscription
{
    public interface ISubscriptionRepository
    {
        IEnumerable<Subscription> GetSubscriptions(long customerNumber, string source, DateTime limitDate);
    }
}