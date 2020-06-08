using System;
using System.Collections.Generic;
using Di.Common.Logging;
using Di.Common.Utils.Url;
using Di.Common.WebRequests;

namespace Di.ServicePlus.RestApi.Requests
{
    public class RequestBase
    {
        private const string AuthorizationHeaderParamName = "authorization";

        private readonly IRequestService _requestService;

        private readonly string _apiUrl;

        public RequestBase(string servicePlusApiUrl) : this(servicePlusApiUrl, new RequestService(new Log4NetLogger())) //todo: kj create factory
        {
        }

        public RequestBase(string servicePlusApiUrl, IRequestService requestService)
        {
            if (string.IsNullOrEmpty(servicePlusApiUrl))
            {
                throw new ArgumentNullException("servicePlusApiUrl");
            }
            if (requestService == null)
            {
                throw new ArgumentNullException("requestService");
            }

            _requestService = requestService;
            _apiUrl = servicePlusApiUrl;
        }

        protected string CreateRequestWithToken(RequestVerb requestVerb, string endPoint, string token, string json = "", Dictionary<string, string> queryParameters = null, string contentType = "application/json", Dictionary<string, string> urlParameters = null)
        {
            if (requestVerb != RequestVerb.Post && urlParameters != null)
            {
                throw new NotImplementedException(requestVerb + " with urlParameters not implemented.");
            }

            switch (requestVerb)
            {
                case RequestVerb.Get:
                    //return _requestService.CreateGetRequestWithHeaderParams(_apiUrl + endPoint, new Dictionary<string, string> { { AuthorizationHeaderParamName, token } });
                    return _requestService.CreateGetRequest(GetApiUrl(endPoint, token, queryParameters), contentType);
                case RequestVerb.Post:
                    return urlParameters != null
                        ? _requestService.CreatePostRequest(GetApiUrl(endPoint, token, queryParameters), urlParameters, contentType) 
                        : _requestService.CreatePostRequest(GetApiUrl(endPoint, token, queryParameters), json, contentType);
                case RequestVerb.Put:
                    return _requestService.CreatePutRequest(GetApiUrl(endPoint, token, queryParameters), json, contentType);
                case RequestVerb.Delete:
                    return _requestService.CreateDeleteRequest(GetApiUrl(endPoint, token, queryParameters), contentType);
                default:
                    throw new NotImplementedException("No implementation");
            }
        }

        protected string CreateRequest(RequestVerb requestVerb, string endPoint, Dictionary<string, string> urlParamsDictinary)
        {
            switch (requestVerb)
            {
                case RequestVerb.Get:
                    throw new NotImplementedException("Get is not implemented in RequestBase");
                case RequestVerb.Post:
                    return _requestService.CreatePostRequest(_apiUrl + endPoint, urlParamsDictinary);
                case RequestVerb.Put:
                    throw new NotImplementedException("Put is not implemented in RequestBase");
                default:
                    return string.Empty;
            }
        }

        private string GetApiUrl(string endPoint, string token, Dictionary<string, string> queryParameters)
        {
            var apiUrl = AddTokenToUrl(_apiUrl + endPoint, token);

            if (queryParameters == null)
            {
                return apiUrl;
            }

            apiUrl = UrlUtils.AddDictionaryToQueryString(apiUrl, queryParameters);

            return apiUrl;
        }

        private static string AddTokenToUrl(string url, string token)
        {
            return url + "?access_token=" + token;
        }
    }
}
