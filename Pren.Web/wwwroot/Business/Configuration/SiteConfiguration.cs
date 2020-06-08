using System.Configuration;
using Di.Common.Logging;

namespace Pren.Web.Business.Configuration
{
    class SiteConfiguration : ISiteConfiguration
    {
        private readonly ILogger _logService;

        public SiteConfiguration(ILogger logService)
        {
            _logService = logService;
        }

        public string GetSetting(string key, string defaultValue = "")
        {
            var setting = ConfigurationManager.AppSettings.Get(key);
            if (!string.IsNullOrEmpty(setting))
            {
                return setting;
            }

            _logService.Log("No appsetting found for key '" + key + "'. Returning defaultvalue '" + defaultValue + "' instead", LogLevel.Error, typeof(SiteConfiguration));            
            return defaultValue;
        }
    }
}
