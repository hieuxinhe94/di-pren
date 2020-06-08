using System.Collections.Generic;
using Di.Subscription.Logic.Initialization;
using Di.Subscription.Logic.Reclaim.Retrievers;
using Di.Subscription.Logic.Reclaim.Types;

namespace Pren.Web.Business.Cache.Reclaim.Retrievers
{
    /// <summary>
    /// The Strategy pattern is used to separate caching logic from data access. 
    /// See how dependencies are configured in <see cref="DependencyResolver"/>
    /// </summary>
    public class CachedReclaimTypeRetriever : IReclaimTypeRetriever
    {
        private readonly IReclaimTypeRetriever _reclaimTypeRetriever;
        private readonly IObjectCache _objectCache;

        public CachedReclaimTypeRetriever(
            IReclaimTypeRetriever reclaimTypeRetriever,
            IObjectCache objectCache)
        {
            _reclaimTypeRetriever = reclaimTypeRetriever;
            _objectCache = objectCache;
        }

        public IEnumerable<ReclaimType> GetReclaimTypes()
        {
            const string cacheKey = "subscriptionReclaimTypes";

            var reclaimTypes = (IEnumerable<ReclaimType>)_objectCache.GetFromCache(cacheKey);

            if (reclaimTypes != null)
            {
                return reclaimTypes;
            }

            reclaimTypes = _reclaimTypeRetriever.GetReclaimTypes();
            _objectCache.AddToCache(cacheKey, reclaimTypes);

            return reclaimTypes;
        }
    }
}
