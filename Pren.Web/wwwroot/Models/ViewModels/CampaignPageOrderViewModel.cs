using System.Collections.Generic;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.Pages;

namespace Pren.Web.Models.ViewModels
{
    public class CampaignPageOrderViewModel : PageViewModel<CampaignPageIframe>
    {
        public CampaignPageOrderViewModel(CampaignPageIframe currentPage) : base(currentPage) { }

        public string TargetGroup { get; set; }
        public string Callback { get; set; }
        public string OfferOrigin { get; set; }
        public bool IsMobileDevice { get; set; }
        public string IframeUrl { get; set; }
        public CampaignBlock SelectedCampaign { get; set; }
        public Dictionary<string, string> Parameters { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> UspTexts { get; set; }
        public string SalesChannel { get; set; }
        public string AppId { get; set; }
    }
}