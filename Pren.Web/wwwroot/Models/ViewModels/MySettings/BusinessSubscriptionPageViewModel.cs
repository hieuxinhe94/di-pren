using Pren.Web.Business.Messaging;
using Pren.Web.Controllers.MySettings;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class BusinessSubscriptionPageViewModel : PageViewModel<BusinessSubscriptionPage>
    {
        public BusinessSubscriptionPageViewModel(BusinessSubscriptionPage currentPage)
            : base(currentPage)
        {
            MasterUser = new BusinessSubscriber();
        }

        public Message Message { get; set; }
        public bool ShowReciept { get; set; }
        public string SubscriptionId { get; set; }
        public string CompanyName { get; set; }
        public int MaxNumberOfAllowedSubscribers { get; set; }
        public int MinNumberOfAllowedSubscribers { get; set; }
        public int AccountPricePerMonth { get; set; }
        public BusinessSubscriber MasterUser { get; set; }
        public long BizSubscriberCustomerNumber { get; set; }
        public string ActiveTab { get; set; }
    }
}
