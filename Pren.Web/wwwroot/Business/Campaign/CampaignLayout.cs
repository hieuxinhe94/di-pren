using EPiServer.Core;

namespace Pren.Web.Business.Campaign
{
    public class CampaignLayout
    {
        public ContentReference ChooseCampaignPage { get; set; }

        public bool ShowDigitalLink { get; set; }

        public XhtmlString DisclaimerText { get; set; }
    }
}