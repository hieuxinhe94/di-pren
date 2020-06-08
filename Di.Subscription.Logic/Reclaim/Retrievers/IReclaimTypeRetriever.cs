using System.Collections.Generic;

namespace Di.Subscription.Logic.Reclaim.Retrievers
{
    public interface IReclaimTypeRetriever
    {
        IEnumerable<Types.ReclaimType> GetReclaimTypes();
    }
}
