using System;
using System.Collections.Generic;
using System.Linq;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.Campaign
{
    internal class CampaignRepository : ICampaignRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public CampaignRepository(ISubscriptionDataAccess subscriptionDataAccess, IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public IEnumerable<CampaignGroup> GetActiveCampaigns(string packageId)
        {
            var activeCampaignsDataSet = _subscriptionDataAccess.GetActiveCampaigns(packageId);
            return _objectConverter.ConvertFromDataSet<CampaignGroup>(activeCampaignsDataSet);
        }

        public Campaign GetCampaign(long campaignNumber)
        {
            var campaignsDataSet = _subscriptionDataAccess.GetCampaign(campaignNumber);
            return _objectConverter.ConvertFromDataSet<Campaign>(campaignsDataSet).FirstOrDefault();
        }

        public CampaignSimple GetCampaignSimple(string campaignId)
        {
            var campaignsDataSet = _subscriptionDataAccess.GetCampaignSimple(campaignId, DateTime.Now);
            return _objectConverter.ConvertFromDataSet<CampaignSimple>(campaignsDataSet).FirstOrDefault();
        }
    }
}