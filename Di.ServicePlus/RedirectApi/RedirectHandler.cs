using System.Text;
using System.Web;
using Di.ServicePlus.RedirectApi.Orders;

namespace Di.ServicePlus.RedirectApi
{
    internal class RedirectHandler : IRedirectHandler
    {
        public IOrders Orders { get; private set; }

        public RedirectHandler(IOrders orders)
        {
            Orders = orders;
        }

        private static string AppIdAndLc { get { return "?appId=" + "di.se" + "&lc=sv"; } } //TODO: KJ Bryt ut i settings

        public string GetCheckedLoginUrl(string callBackUrl)
        {
            var sb = new StringBuilder();
            sb.Append(Settings.ServicePlusLoginPageUrl);
            sb.Append("check-logged-in");
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBackUrl));
            sb.Append("&bipPrompt=true");
            return sb.ToString();
        }

        public string GetLoginUrl(string callBackUrl)
        {
            var sb = new StringBuilder();
            sb.Append(Settings.ServicePlusLoginPageUrl);
            sb.Append("login");
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBackUrl));
            return sb.ToString();
        }

        public string GetLogoutUrl(string callBackUrl)
        {
            var sb = new StringBuilder();
            sb.Append(Settings.ServicePlusLoginPageUrl);
            sb.Append("logout");
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBackUrl));
            return sb.ToString();
        }

        public string GetCreateAccountUrl(string callBackUrl)
        {
            var sb = new StringBuilder();
            sb.Append(Settings.ServicePlusLoginPageUrl);
            sb.Append("register");
            sb.Append(AppIdAndLc);
            sb.Append("&callback=" + HttpUtility.UrlEncode(callBackUrl));
            return sb.ToString();
        }
    }
}