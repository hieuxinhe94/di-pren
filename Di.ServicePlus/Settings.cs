using System.Configuration;

namespace Di.ServicePlus
{
    internal class Settings
    {
        //TODO: KJ skicka in url istället
        public static string ServicePlusLoginPageUrl
        {
            get { return ConfigurationManager.AppSettings.Get("ServicePlusLoginPageUrl"); }
        }
    }
}
