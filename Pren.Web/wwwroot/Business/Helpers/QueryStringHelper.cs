using System.Web;
using EPiServer.Shell.Web;

namespace Pren.Web.Business.Helpers
{
    public class QueryStringHelper : IQueryStringHelper
    {
        public string Get(string key, HttpContextBase context = null)
        {
            var httpContext = context ?? HttpContext.Current.GetHttpContextBase();

            return httpContext.Request.QueryString[key];
        }

        public string[] GetAll(HttpContextBase context = null)
        {
            var httpContext = context ?? HttpContext.Current.GetHttpContextBase();

            return httpContext.Request.QueryString.AllKeys;
        }
    }
}
