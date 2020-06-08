using Pren.Web.Business.DataAccess.Campaign.Entities;

namespace Pren.Web.Business.DataAccess.Campaign
{
    public interface ICampaignDataHandler
    {
        void AddOrUpdateCampaign(string campaignId, int campaignNumber, string packageId, string paperCode, string productNumber, decimal totalPriceExcludningVat, decimal totalPriceIncludningVat, decimal vatAmount, decimal vatPercent, string priceGroup, string subsKind, decimal priceForCustomerToPay);

        CampaignDataEntity GetCampaign(int campaignNumber, string campaignId);
    }
}