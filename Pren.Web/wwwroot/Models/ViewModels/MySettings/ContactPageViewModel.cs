using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class ContactPageViewModel : PageViewModel<ContactPage>
    {
        public ContactPageViewModel(ContactPage currentPage)
            : base(currentPage)
        {
            
        }

        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool HideForm { get; set; }
        public long CustomerNumber { get; set; }
    }
}
