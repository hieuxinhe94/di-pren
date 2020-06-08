using System.Web;

namespace Pren.Web.Business.Session
{
    public interface ISessionData
    {
        void Set(string key, object value, HttpContextBase context = null);
        object Get(string key, HttpContextBase context = null);
    }
}
