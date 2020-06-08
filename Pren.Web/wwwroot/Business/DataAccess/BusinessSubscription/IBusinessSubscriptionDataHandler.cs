namespace Pren.Web.Business.DataAccess.BusinessSubscription
{
    public interface IBusinessSubscriptionDataHandler
    {
        void AddOrUpdatePrice(string externalProductCode, int price);

        int GetPrice(string externalProductCode);
    }
}
