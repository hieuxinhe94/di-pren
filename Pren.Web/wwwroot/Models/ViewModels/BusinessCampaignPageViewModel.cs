using System.Collections.Generic;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;

namespace Pren.Web.Models.ViewModels
{
    public class BusinessCampaignPageViewModel : PageViewModel<BusinessCampaignPage>
    {
        public BusinessCampaignPageViewModel(BusinessCampaignPage currentPage)
            : base(currentPage)
        {          
            SubscriptionForm = new BusinessCampaignSubscribeFormModel();           
        }

        public BusinessCampaignSubscribeFormModel SubscriptionForm { get; set; }
        public string LoginUrl { get; set; }
        public string LogOutUrl { get; set; }
        public IEnumerable<BusinessOffer> BusinessOffers { get; set; }
        public string ErrorMessage { get; set; }
        public List<string> UspTexts { get; set; }
        public string CampaignId { get; set; }
    }

    public class BusinessOffer
    {        
        public string Id { get; set; }
        public string CampaignNumber { get; set; }
        public int MinAccounts { get; set; }
        public int MaxAccounts { get; set; }
        public int PricePerAccount { get; set; }
    }
}
