using System;
using System.Collections.Generic;

namespace Di.Subscription.Logic.Subscription
{
    public interface ISubscriptionHandler
    {
        IEnumerable<Types.Subscription> GetSubscriptions(long customerNumber, DateTime limitDate);
    }
}