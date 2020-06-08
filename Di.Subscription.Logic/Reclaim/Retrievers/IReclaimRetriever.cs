using System.Collections.Generic;

namespace Di.Subscription.Logic.Reclaim.Retrievers
{
    public interface IReclaimRetriever
    {
        IEnumerable<Types.Reclaim> GetCustomerReclaims(long customerNumber);
    }
}
