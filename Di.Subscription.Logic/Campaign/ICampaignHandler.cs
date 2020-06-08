using System.Collections.Generic;

namespace Di.Subscription.Logic.Campaign
{
    public interface ICampaignHandler
    {
        IEnumerable<Types.CampaignIdentifier> GetActiveCampignIdentifiers(bool forceRefresh = false);
        Types.Campaign GetCampaign(long campaignNumber);
        Types.Campaign GetCampaign(string campaignId);
    }
}