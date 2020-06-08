
using EPiServer.Core;

namespace Pren.Web.Business.MyPage
{
    public class MySettingsLayout
    {
        public string LogInUrl { get; set; }

        public string LogOutUrl { get; set; }

        public string CreateAccountUrl { get; set; }

        public bool IsLoggedIn { get; set; }

        public bool HideDbFunctions { get; set; }

        public string UserName { get; set; }

        public IContent StartPage { get; set; }

        public ContentArea FooterContentArea { get; set; }
    }
}