using System.Collections.Generic;
using Di.Subscription.Logic.Campaign.Retrievers;
using Di.Subscription.Logic.Campaign.Types;

namespace Di.Subscription.Logic.Campaign
{
    internal class CampaignHandler : ICampaignHandler
    {
        private readonly ICampaignIdentifierRetriver _campaignIdentifierRetriver;
        private readonly ICampaignRetriever _campaignRetriever;

        public CampaignHandler(
            ICampaignIdentifierRetriver campaignIdentifierRetriver,
            ICampaignRetriever campaignRetriever)
        {
            _campaignIdentifierRetriver = campaignIdentifierRetriver;
            _campaignRetriever = campaignRetriever;
        }

        public IEnumerable<CampaignIdentifier> GetActiveCampignIdentifiers(bool forceRefresh = false)
        {
            return _campaignIdentifierRetriver.GetActiveCampignIdentifiers(forceRefresh);
        }

        public Types.Campaign GetCampaign(long campaignNumber)
        {
            return _campaignRetriever.GetCampaign(campaignNumber);
        }

        public Types.Campaign GetCampaign(string campaignId)
        {
            return _campaignRetriever.GetCampaign(campaignId);
        }
    }
}