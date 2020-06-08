using Di.Common.Logging;

namespace Di.Common.Utils.Context
{
    public class UserContext : Loggable
    {
        public UserContext()
        {
            UserAgent = HttpContextUtils.GetUserAgent();
            UserBrowser = HttpContextUtils.GetUserBrowser();
            UserBrowserVersion = HttpContextUtils.GetUserBrowserVersion();
            UserIp = HttpContextUtils.GetUserIp();
            IsMobileDevice = HttpContextUtils.IsMobileDevice();
        }

        public string UserAgent { get; private set; }

        public string UserBrowser { get; private set; }

        public string UserBrowserVersion { get; private set; }

        public string UserIp { get; private set; }

        public bool IsMobileDevice { get; private set; }
    }
}
