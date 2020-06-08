using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels.Auth;
using Di.Common.Cache;
using Di.Common.Logging;
using Di.Common.Utils.Url;

namespace Bn.Subscription.RestApi.Requests
{
    public abstract class RequestBase
    {
        private readonly IRequestService _requestService;
        private readonly IObjectCache _objectCache;
        private readonly string _apiUrl;
        private readonly string _apiClient;
        private readonly string _apiSecret;

        protected RequestBase(string apiUrl, string apiClient, string apiSecret) : this(apiUrl, apiClient, apiSecret, new RequestService(new Log4NetLogger()), new ObjectCache()) 
        {

        }

        protected RequestBase(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache)
        {
            if (string.IsNullOrEmpty(apiUrl))
            {
                throw new ArgumentNullException(nameof(apiUrl));
            }
            if (string.IsNullOrEmpty(apiClient))
            {
                throw new ArgumentNullException(nameof(apiClient));
            }
            if (string.IsNullOrEmpty(apiSecret))
            {
                throw new ArgumentNullException(nameof(apiSecret));
            }
            if (requestService == null)
            {
                throw new ArgumentNullException(nameof(requestService));
            }
            if (objectCache == null)
            {
                throw new ArgumentNullException(nameof(objectCache));
            }

            _requestService = requestService;
            _objectCache = objectCache;
            _apiUrl = apiUrl;
            _apiClient = apiClient;
            _apiSecret = apiSecret;
        }

        #region GET

        protected async Task<T> GetAsync<T>(string endPoint)
        {
            return await GetAsync<T>(endPoint, null);
        }

        protected async Task<T> GetAsync<T>(string endPoint, Dictionary<string, string> queryParameters)
        {
            return await CreateAuthenticatedRequest(
                async (requestService, token) => await requestService.GetAsync<T>(GetApiUrl(endPoint, queryParameters), token));
        }

        #endregion

        #region DELETE

        protected async Task<T> DeleteAsync<T>(string endPoint)
        {
            return await DeleteAsync<T>(endPoint, null);
        }

        protected async Task<T> DeleteAsync<T>(string endPoint, Dictionary<string, string> queryParameters)
        {
            return await CreateAuthenticatedRequest(
                async (requestService, token) => await requestService.DeleteAsync<T>(GetApiUrl(endPoint, queryParameters), token));
        }

        #endregion

        #region POST

        protected async Task<T> PostAsync<T>(string endPoint)
        {
            return await PostAsync<T>(endPoint, null, null);
        }

        protected async Task<T> PostAsync<T>(string endPoint, Dictionary<string, string> queryParameters)
        {
            return await PostAsync<T>(endPoint, queryParameters, null);
        }

        protected async Task<T> PostAsync<T>(string endPoint, HttpContent content)
        {
            return await PostAsync<T>(endPoint, null, content);
        }

        protected async Task<T> PostAsync<T>(string endPoint, Dictionary<string, string> queryParameters, HttpContent content)
        {
            return await CreateAuthenticatedRequest(
                async (requestService, token) => await requestService.PostAsync<T>(GetApiUrl(endPoint, queryParameters), content, token));
        }

        protected async Task<TOutput> PostAsJsonAsync<TInput, TOutput>(string endPoint, TInput value)
        {
            return await CreateAuthenticatedRequest(
                async (requestService, token) => await requestService.PostAsJsonAsync<TInput, TOutput>(GetApiUrl(endPoint), value, token));
        }

        #endregion

        #region PUT

        protected async Task<T> PutAsync<T>(string endPoint)
        {
            return await PutAsync<T>(endPoint, null, null);
        }

        protected async Task<T> PutAsync<T>(string endPoint, Dictionary<string, string> queryParameters)
        {
            return await PutAsync<T>(endPoint, queryParameters, null);
        }

        protected async Task<T> PutAsync<T>(string endPoint, HttpContent content)
        {
            return await PutAsync<T>(endPoint, null, content);
        }

        protected async Task<T> PutAsync<T>(string endPoint, Dictionary<string, string> queryParameters, HttpContent content)
        {
            return await CreateAuthenticatedRequest(
                async (requestService, token) => await requestService.PutAsync<T>(GetApiUrl(endPoint, queryParameters), content, token));
        }

        protected async Task<TOutput> PutAsJsonAsync<TInput, TOutput>(string endPoint, TInput value)
        {
            return await CreateAuthenticatedRequest(
                async (requestService, token) => await requestService.PutAsJsonAsync<TInput, TOutput>(GetApiUrl(endPoint), value, token));
        }

        #endregion

        #region privates

        private string GetApiUrl(string endPoint,  Dictionary<string, string> queryParameters = null)
        {
            var apiUrl = _apiUrl + endPoint;

            if (queryParameters == null)
            {
                return apiUrl;
            }

            apiUrl = UrlUtils.AddDictionaryToQueryString(apiUrl, queryParameters);

            return apiUrl;
        }


        private async Task<T> CreateAuthenticatedRequest<T>(Func<IRequestService, string, Task<T>>  request)
        {
            try
            {
                var token = await GetSystemAccessToken();
                return await request(_requestService, token.AccessToken);
            }
            catch (AuthenticationException)
            {
                // If token is invalid, get a new one and try again
                var retryToken = await GetSystemAccessToken(false);
                return await request(_requestService, retryToken.AccessToken);
            }
        }

        private async Task<Token> GetSystemAccessToken(bool fromCache = true)
        {
            const string cacheKey = "PrenSystemAccessToken";

            if (fromCache)
            {
                var systemToken = (Token) _objectCache.Get(cacheKey);

                if (systemToken != null && systemToken.Expires >= DateTime.Now)
                {
                    return systemToken;
                }
            }

            var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("client_id", _apiClient),
                    new KeyValuePair<string, string>("client_secret", _apiSecret),
                    new KeyValuePair<string, string>("grant_type", "client_credentials")
                });

            var token = await _requestService.PostAsync<Token>(GetApiUrl("oauth/token"), content);

            _objectCache.Insert(cacheKey, token);

            return token;
        }

        #endregion

    }
}
