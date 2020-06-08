using EPiServer.Core;

namespace Pren.Web.Business.Session
{
    public interface IPageSpecificSession
    {
        void AddQueryParameterToSession(IContent currentPage, string key, string value);
        string GetQueryParameterFromSession(IContent currentPage, string key);
        TType GetFromSession<TType>(IContent currentPage, string sessionKey);
        void SetInSession(IContent currentPage, string sessionKey, object value);
        void ClearSession(IContent currentPage, string sessionKey);
    }
}
