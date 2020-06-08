using System.Collections.Generic;
using Pren.Web.Models.CustomForms;
using Pren.Web.Models.Pages;

namespace Pren.Web.Models.ViewModels
{
    public class BusinessSubscriptionActivationPageViewModel : PageViewModel<BusinessSubscriptionActivationPage>
    {
        public BusinessSubscriptionActivationPageViewModel(BusinessSubscriptionActivationPage currentPage)
            : base(currentPage)
        {
            ActivationForm = new BusinessSubscriptionActivationFormModel();
        }

        public string InvitingCompanyName { get; set; }
        public BusinessSubscriptionActivationFormModel ActivationForm { get; set; }
        public bool DisplayErrorMessage { get; set; }
        public bool InviteExpired { get; set; }
        public List<string> UspTexts { get; set; }
    }
}
