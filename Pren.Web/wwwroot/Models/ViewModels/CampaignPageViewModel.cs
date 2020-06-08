using Pren.Web.Models.Blocks;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;

namespace Pren.Web.Models.ViewModels
{
    public class CampaignPageViewModel : PageViewModel<CampaignPage>
    {
        public CampaignPageViewModel(CampaignPage currentPage) : base(currentPage)
        {          
            SubscriptionForm = new CampaignSubscribeFormModel();           
        }

        public CampaignBlock SelectecedCampaign { get; set; }
        public CampaignSubscribeFormModel SubscriptionForm { get; set; }
        public string LoginUrl { get; set; }
        public string LogOutUrl { get; set; }
        public string ErrorMessage { get; set; }
        public bool IsMobileDevice { get; set; }
        public string BipMessage { get; set; }
    }
}
