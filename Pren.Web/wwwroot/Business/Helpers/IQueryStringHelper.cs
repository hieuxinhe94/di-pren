using System.Web;

namespace Pren.Web.Business.Helpers
{
    public interface IQueryStringHelper
    {
        string Get(string key, HttpContextBase context = null);

        string[] GetAll(HttpContextBase context = null);
    }
}
