using System;
using System.Collections.Generic;
using Di.Subscription.Logic.HolidayStop.Types;
using Pren.Web.Business.Messaging;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class SubscriptionSleepPageViewModel : PageViewModel<SubscriptionSleepPage>
    {
        public SubscriptionSleepPageViewModel(SubscriptionSleepPage currentPage) : base(currentPage)
        {
            SubscriptionSleepForm = new SubscriptionSleepFormModel();
        }
        public DateTime DateNotSet { get; set; }   
        public long SubscriptionNumber { get; set; }
        public List<HolidayStopItem> FutureSubsSleeps { get; set; }

        public Message Message { get; set; }

        public SubscriptionSleepFormModel SubscriptionSleepForm { get; set; }
    }
}
