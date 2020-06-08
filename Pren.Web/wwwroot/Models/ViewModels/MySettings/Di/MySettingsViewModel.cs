using System.Collections.Generic;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.Pages.MySettings.Di;

namespace Pren.Web.Models.ViewModels.MySettings.Di
{
    public class MySettingsViewModel : PageViewModel<MyStartPage>
    {
        public MySettingsViewModel(MyStartPage currentPage)
            : base(currentPage)
        {                   
        }

        public string UserName { get; set; }

        public bool IsDebug { get; set; }
        public bool IsLoggedIn { get; set; }

        public string Anchor { get; set; }

        public List<TopMenuItem> TopMenuItems { get; set; }
        public BusinessSubscriptionPage BusinessSubscriptionPage { get; set; }
    }

    public class TopMenuItem
    {
        public TopMenuItem(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
