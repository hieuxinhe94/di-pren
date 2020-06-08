using System;
using System.Collections.Generic;
using Pren.Web.Business.Messaging;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class ComplaintPageViewModel : PageViewModel<ComplaintPage>
    {
        public ComplaintPageViewModel(ComplaintPage currentPage) : base(currentPage)
        {
        }

        public IEnumerable<ReclaimItem> ReclaimItems { get; set; }

        public IEnumerable<DateTime> DaysToReclaim { get; set; }

        public string SubscriptionNumber { get; set; }
    }

    public class ReclaimItem
    {
        public ReclaimItem(string text, string value)
        {
            this.Text = text;
            this.Value = value;
        }

        public string Text { get; set; }
        public string Value { get; set; }

    }
}
