using Di.ServicePlus;
using Pren.Web.Business.Configuration;

namespace Pren.Web.Business.ServicePlus.Logic
{
    public interface IServicePlus
    {
        IServicePlusApi RestApi { get; }
    }

    public class ServicePlus : IServicePlus
    {
        public IServicePlusApi RestApi { get; private set; }

        public ServicePlus(ISiteSettings siteSettings)
        {
            RestApi = new ServicePlusApi(siteSettings.ServicePlusApiBaseUrl);
        }
    }
}
