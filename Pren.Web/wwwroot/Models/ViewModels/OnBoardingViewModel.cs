using System.Collections.Generic;
using Pren.Web.Models.Pages;
using Pren.Web.Models.Partials.OrderFlow;

namespace Pren.Web.Models.ViewModels
{
    public class OnBoardingViewModel : PageViewModel<OnBoardingCampaignPage>
    {
        public OnBoardingViewModel(OnBoardingCampaignPage currentPage) : base(currentPage) { }

        public PackageModel Package { get; set; }

        public Dictionary<string, string> Parameters { get; set; }

        public string IframeUrl { get; set; }

        public string CallbackUrl { get; set; }

        public string BaseUrl { get; set; }
    }
}