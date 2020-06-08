using Pren.Web.Models.Pages.MySettings;

namespace Pren.Web.Models.ViewModels.MySettings
{
    public class CodePortalPageViewModel : PageViewModel<CodePortalPage>
    {
        public CodePortalPageViewModel(CodePortalPage currentPage)
            : base(currentPage)
        {
            
        }

        public string UserId { get; set; }
        public string Token { get; set; }
    }
}
