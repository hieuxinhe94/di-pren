using System.Collections.Generic;

namespace Di.Subscription.DataAccess.Campaign
{
    public interface ICampaignRepository
    {
        IEnumerable<CampaignGroup> GetActiveCampaigns(string packageId);

        Campaign GetCampaign(long campaignNumber);

        CampaignSimple GetCampaignSimple(string campaignId);
    }
}
