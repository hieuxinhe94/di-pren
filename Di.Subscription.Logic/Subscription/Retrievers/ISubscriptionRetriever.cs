using System;
using System.Collections.Generic;

namespace Di.Subscription.Logic.Subscription.Retrievers
{
    internal interface ISubscriptionRetriever
    {
        IEnumerable<Types.Subscription> GetSubscriptions(long customerNumber, string source, DateTime limitDate);
    }
}
