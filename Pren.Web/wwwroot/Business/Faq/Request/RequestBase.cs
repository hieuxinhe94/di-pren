using System;
using System.Collections.Generic;
using System.Globalization;
using Di.Common.Logging;
using Di.Common.Utils.Url;
using Di.Common.WebRequests;

namespace Pren.Web.Business.Faq.Request
{
    public class RequestBase
    {
        private readonly IRequestService _requestService;

        private readonly string _apiUrl;

        public RequestBase(string faqApiUrl) : this(faqApiUrl, new RequestService(new Log4NetLogger())) //todo: kj create factory
        {
        }

        public RequestBase(string faqApiUrl, IRequestService requestService)
        {
            if (string.IsNullOrEmpty(faqApiUrl))
            {
                throw new ArgumentNullException("faqApiUrl");
            }
            if (requestService == null)
            {
                throw new ArgumentNullException("requestService");
            }

            _requestService = requestService;
            _apiUrl = faqApiUrl;
        }

        protected Dictionary<string, string> GetQueryStringDictionary(int limit, string sortOrder)
        {
            return new Dictionary<string, string>
            {
                {"limit", limit.ToString(CultureInfo.InvariantCulture)},
                {"sort", sortOrder}
            };
        }

        protected string CreateRequest(RequestVerb requestVerb, string endPoint, Dictionary<string, string> urlParamsDictinary = null)
        {
            var url = GetApiUrl(endPoint, urlParamsDictinary);

            switch (requestVerb)
            {
                case RequestVerb.Get:
                    return _requestService.CreateGetRequest(url);
                case RequestVerb.Post:
                    throw new NotImplementedException("Post is not implemented in RequestBase");
                case RequestVerb.Put:
                    throw new NotImplementedException("Put is not implemented in RequestBase");
                case RequestVerb.Delete:
                    throw new NotImplementedException("Delete is not implemented in RequestBase");
                default:
                    return string.Empty;
            }
        }

        private string GetApiUrl(string endPoint, Dictionary<string, string> queryParameters)
        {
            var url = _apiUrl + endPoint;

            if (queryParameters == null)
                return url;

            url = UrlUtils.AddDictionaryToQueryString(url, queryParameters);

            return url;
        }
    }
}
