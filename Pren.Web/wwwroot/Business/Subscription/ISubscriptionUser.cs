namespace Pren.Web.Business.Subscription
{
    public interface ISubscriptionUser<TSubscriptionUser>
    {
        bool HasMultipleCustomerNumberInServicePlus { get; set; }

        bool HasCustomerNumberInServicePlus { get; set; }

        bool IsLoggedInServicePlus { get; set; }        

        bool IsConnected { get; set; }

        bool InvalidCode { get; set; }

        string Token { get; set; }

        string Code { get; set; }

        string ServicePlusUserName { get; set; }

        string ServicePlusEmail { get; set; }

        long BizSubscriptionCustomerNumber { get; set; }

        TSubscriptionUser Subscriber { get; set; }

        ServicePlus.Models.User ServicePlusUser { get; set; }

        bool IsBizSubscriptionAdmin { get; set; }

        bool IsPendingSubscriber { get; set; }

        bool IsPrivateSubscriber { get; set; }

        long PrivateSubscriberCustomerNumber { get; set; }
    }
}