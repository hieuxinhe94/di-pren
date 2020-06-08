using EPiServer;
using Pren.Web.Models.Pages;

namespace Pren.Web.Models.ViewModels
{
    public class CampaignLandingPageViewModel : PageViewModel<CampaignLandingPage>
    {
        public CampaignLandingPageViewModel(CampaignLandingPage currentPage) : base(currentPage)
        {

        }

        public Url ButtonLeftUrl { get; set; }

        public Url ButtonRightUrl { get; set; }
    }
}
