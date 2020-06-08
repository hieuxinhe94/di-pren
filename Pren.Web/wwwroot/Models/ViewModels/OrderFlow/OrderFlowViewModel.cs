using System.Collections.Generic;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;

namespace Pren.Web.Models.ViewModels.OrderFlow
{
    public class OrderFlowViewModel : PageViewModel<OrderFlowCampaignPage>
    {
        public OrderFlowViewModel(OrderFlowCampaignPage currentPage) : base(currentPage) { }

        public List<PackageModel> Packages { get; set; }

        public string QueryString { get; set; }

        public string PageUrl { get; set; }
    }
}