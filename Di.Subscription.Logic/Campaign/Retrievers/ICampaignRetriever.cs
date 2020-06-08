namespace Di.Subscription.Logic.Campaign.Retrievers
{
    public interface ICampaignRetriever
    {
        Types.Campaign GetCampaign(long campaignNumber);
        Types.Campaign GetCampaign(string campaignId);
    }
}