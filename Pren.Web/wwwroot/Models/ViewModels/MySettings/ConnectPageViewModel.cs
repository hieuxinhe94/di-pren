using Pren.Web.Business.Messaging;
using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class ConnectPageViewModel : PageViewModel<ConnectPage>
    {
        public ConnectPageViewModel(ConnectPage currentPage)
            : base(currentPage)
        {
            
        }

        public long CustomerNumberPren { get; set; }
        public string CustomerNamePren { get; set; }

        public string CustomerEmailServicePlus { get; set; }
        public string CustomerNameServicePlus { get; set; }

        public bool HideForms { get; set; }

        public Message Message { get; set; }
    }
}
