using System.Web;
using EPiServer.Shell.Web;

namespace Pren.Web.Business.Session
{
    public class SessionData : ISessionData
    {
        public void Set(string key, object value, HttpContextBase context = null)
        {            
            var httpContext = context ?? HttpContext.Current.GetHttpContextBase();

            if (httpContext.Session != null)
            {
                httpContext.Session.Add(key, value);
            }            
        }

        public object Get(string key, HttpContextBase context = null)
        {
            var httpContext = context ?? HttpContext.Current.GetHttpContextBase();

            return httpContext.Session != null ? httpContext.Session[key] : null;
        }
    }
}
