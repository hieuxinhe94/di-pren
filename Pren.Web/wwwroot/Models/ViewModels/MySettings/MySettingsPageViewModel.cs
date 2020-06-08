using Pren.Web.Business.Messaging;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class MySettingsPageViewModel : PageViewModel<MySettingsStartPage>
    {
        public MySettingsPageViewModel(MySettingsStartPage currentPage) : base(currentPage)
        {                   
        }

        public Message Message { get; set; }
    }
}
