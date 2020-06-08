using System.Collections.Generic;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;

namespace Pren.Web.Models.ViewModels.OrderFlow
{
    public class OrderFlowOrderViewModel : PageViewModel<OrderFlowCampaignPage>
    {
        public OrderFlowOrderViewModel(OrderFlowCampaignPage currentPage) : base(currentPage) { }

        public PackageModel Package { get; set; }

        public string QueryString { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public string IframeUrl { get; set; }

        public string TargetGroup { get; set; }

        public bool HideChangePackageBtn { get; set; }
    }
}