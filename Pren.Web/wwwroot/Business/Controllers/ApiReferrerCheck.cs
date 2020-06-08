using DIClassLib.Misc;
using System.Linq;

namespace Pren.Web.Business.Controllers
{
    public class ApiReferrerCheck : IApiReferrerCheck
    {
        public bool VerifyDomain(string domainReferrer)
        {   
            var domain = MiscFunctions.GetAppsettingsValue("domain");

            var domainArray = domain.Split(','); 

            return domainArray.Any(t => t == domainReferrer);
        }
    }
}