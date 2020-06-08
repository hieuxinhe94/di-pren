using Pren.Web.Business.Messaging;
using Pren.Web.Models.CustomForms.MySettings;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class PersonInfoViewModel : PageViewModel<PersonInfoPage>
    {
        public PersonInfoViewModel(PersonInfoPage currentPage) : base(currentPage)
        {
            PersonInfoForm = new PersonInfoFormModel();
        }

        public Message Message { get; set; }

        public PersonInfoFormModel PersonInfoForm { get; set; }

        public string ServicePlusEmail { get; set; }

        public string ServicePlusForgotPasswordUrl { get; set; }

        public string ServicePlusChangePasswordUrl { get; set; }
    }
}
