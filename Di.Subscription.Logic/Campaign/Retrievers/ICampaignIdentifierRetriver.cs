using System.Collections.Generic;

namespace Di.Subscription.Logic.Campaign.Retrievers
{
    public interface ICampaignIdentifierRetriver
    {
        IEnumerable<Types.CampaignIdentifier> GetActiveCampignIdentifiers(bool forceRefresh = false);
    }
}