using EPiServer.Core;
using Pren.Web.Business.Campaign;
using Pren.Web.Business.MyPage;

namespace Pren.Web.Models.ViewModels
{
    public class LayoutModel
    {
        public string Copyright { get; set; }
        public XhtmlString PrenTermsText { get; set; }
        public ContentArea FooterContentArea { get; set; }
        public XhtmlString ChatText { get; set; }
        public MySettingsLayout MySettings { get; set; }
        public CampaignLayout Campaign { get; set; }

        public bool IsMobile { get; set; }
        public string OptimizelyDesktopScriptSrc { get; set; }
        public string OptimizelyMobileScriptSrc { get; set; }
    }
}
