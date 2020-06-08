using DIClassLib.Subscriptions;
using Pren.Web.Business.ServicePlus.Models;

namespace Pren.Web.Business.Subscription
{
    public class SubscriptionUser : ISubscriptionUser<SubscriptionUser2>
    {
        public bool HasMultipleCustomerNumberInServicePlus { get; set; }
        public bool HasCustomerNumberInServicePlus { get; set; }
        public bool IsLoggedInServicePlus { get; set; }
        public bool IsConnected { get; set; }
        public bool InvalidCode { get; set; }
        public string Token { get; set; }
        public string Code { get; set; }
        public string ServicePlusUserName { get; set; }
        public string ServicePlusEmail { get; set; }
        public long BizSubscriptionCustomerNumber { get; set; }
        public SubscriptionUser2 Subscriber { get; set; }
        public User ServicePlusUser { get; set; }
        public bool IsBizSubscriptionAdmin { get; set; }
        public bool IsPendingSubscriber { get; set; }
        public bool IsPrivateSubscriber { get; set; }
        public long PrivateSubscriberCustomerNumber { get; set; }
    }
}