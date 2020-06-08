using System.Collections.Generic;
using Di.Subscription.Logic.Campaign.Retrievers;
using Di.Subscription.Logic.Campaign.Types;
using Di.Subscription.Logic.Initialization;

namespace Pren.Web.Business.Cache.Campaign.Retrievers
{
    /// <summary>
    /// The Strategy pattern is used to separate caching logic from data access. 
    /// See how dependencies are configured in <see cref="DependencyResolver"/>
    /// </summary>
    public class CachedCampaignIdentifierRetriver : ICampaignIdentifierRetriver
    {
        private readonly ICampaignIdentifierRetriver _campaignIdentifierRetriver;
        private readonly IObjectCache _objectCache;

        public CachedCampaignIdentifierRetriver(
            ICampaignIdentifierRetriver campaignIdentifierRetriver,
            IObjectCache objectCache)
        {
            _campaignIdentifierRetriver = campaignIdentifierRetriver;
            _objectCache = objectCache;
        }

        public IEnumerable<CampaignIdentifier> GetActiveCampignIdentifiers(bool forceRefresh)
        {
            const string cacheKey = "subscriptionActiveCampignIdentifiers";

            var activeCampignIdentifiers = (IEnumerable<CampaignIdentifier>)_objectCache.GetFromCache(cacheKey);

            if (activeCampignIdentifiers != null && !forceRefresh)
            {
                return activeCampignIdentifiers;
            }

            activeCampignIdentifiers = _campaignIdentifierRetriver.GetActiveCampignIdentifiers();
            _objectCache.AddToCache(cacheKey, activeCampignIdentifiers);

            return activeCampignIdentifiers;
        }
    }
}