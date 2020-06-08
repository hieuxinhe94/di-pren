using System.Collections.Generic;
using System.Web;
using EPiServer.Core;

namespace Pren.Web.Business.Session
{
    public class PageSpecificSession : IPageSpecificSession
    {
        private readonly ISessionData _sessionData;

        public PageSpecificSession(ISessionData sessionData)
        {
            _sessionData = sessionData;
        }

        public void AddQueryParameterToSession(IContent currentPage, string key, string value)
        {
            var queryParameterDictionary = GetFromSession<Dictionary<string, string>>(currentPage, SessionConstants.QueryParameterDictionarySessionKey)
                ?? new Dictionary<string, string>();

            queryParameterDictionary.Remove(key);
            queryParameterDictionary.Add(key, value);
            SetInSession(currentPage, SessionConstants.QueryParameterDictionarySessionKey, queryParameterDictionary);
        }

        public string GetQueryParameterFromSession(IContent currentPage, string key)
        {
            var queryParameterDictionary = GetFromSession<Dictionary<string, string>>(currentPage, SessionConstants.QueryParameterDictionarySessionKey);

            if (queryParameterDictionary != null && queryParameterDictionary.ContainsKey(key))
            {
                return HttpUtility.UrlDecode(queryParameterDictionary[key]);
            }

            return string.Empty;
        }

        public TType GetFromSession<TType>(IContent currentPage, string sessionKey)
        {
            return (TType)_sessionData.Get(GetSessionPrefix(currentPage) + sessionKey);
        }

        public void SetInSession(IContent currentPage, string sessionKey, object value)
        {
            _sessionData.Set(GetSessionPrefix(currentPage) + sessionKey, value);
        }

        public void ClearSession(IContent currentPage, string sessionKey)
        {
            SetInSession(currentPage, sessionKey, null);
        }

        private string GetSessionPrefix(IContent content)
        {
            return content.ContentLink.ID + "_";
        }
    }
}
