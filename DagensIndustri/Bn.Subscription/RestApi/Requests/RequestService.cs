using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Threading.Tasks;
using Di.Common.Logging;

namespace Bn.Subscription.RestApi.Requests
{
    public class RequestService : IRequestService
    {
        private readonly ILogger _logService;

        public RequestService(ILogger logService)
        {
            _logService = logService;
        }

        #region GET

        public async Task<T> GetAsync<T>(string url)
        {
            return await GetAsync<T>(url, null);
        }

        public async Task<T> GetAsync<T>(string url, string token)
        {
            var getResult = await CreateRequest<T>("GET", url, token, async client => await client.GetAsync(url));

            return getResult;
        }

        #endregion

        #region DELETE

        public async Task<T> DeleteAsync<T>(string url)
        {
            return await DeleteAsync<T>(url, null);
        }

        public async Task<T> DeleteAsync<T>(string url, string token)
        {
            var postJsonResult = await CreateRequest<T>("DELETE", url, token, async client => await client.DeleteAsync(url));

            return postJsonResult;
        }

        #endregion

        #region POST

        public async Task<T> PostAsync<T>(string url)
        {
            return await PostAsync<T>(url, null, null);
        }

        public async Task<T> PostAsync<T>(string url, string token)
        {
            return await PostAsync<T>(url, null, token);
        }

        public async Task<T> PostAsync<T>(string url, HttpContent content)
        {
            return await PostAsync<T>(url, content, null);
        }

        public async Task<T> PostAsync<T>(string url, HttpContent content, string token)
        {
            var postResult = await CreateRequest<T>("POST", url, token, async client => await client.PostAsync(url, content));

            return postResult;
        }

        public async Task<TOutput> PostAsJsonAsync<TInput, TOutput>(string url, TInput value)
        {
            return await PostAsJsonAsync<TInput, TOutput>(url, value, null);
        }

        public async Task<TOutput> PostAsJsonAsync<TInput, TOutput>(string url, TInput value, string token)
        {
            var postJsonResult = await CreateRequest<TOutput>("POST", url, token, async client => await client.PostAsJsonAsync(url, value));

            return postJsonResult;
        }

        #endregion

        #region PUT

        public async Task<T> PutAsync<T>(string url)
        {
            return await PutAsync<T>(url, null, null);
        }

        public async Task<T> PutAsync<T>(string url, string token)
        {
            return await PutAsync<T>(url, null, token);
        }

        public async Task<T> PutAsync<T>(string url, HttpContent content)
        {
            return await PutAsync<T>(url, content, null);
        }

        public async Task<T> PutAsync<T>(string url, HttpContent content, string token)
        {
            var putResult = await CreateRequest<T>("PUT", url, token, async client => await client.PutAsync(url, content));

            return putResult;
        }

        public async Task<TOutput> PutAsJsonAsync<TInput, TOutput>(string url, TInput value)
        {
            return await PutAsJsonAsync<TInput, TOutput>(url, value, null);
        }

        public async Task<TOutput> PutAsJsonAsync<TInput, TOutput>(string url, TInput value, string token )
        {
            var putJsonResult = await CreateRequest<TOutput>("PUT", url, token, async client => await client.PutAsJsonAsync(url, value));

            return putJsonResult;
        }

        #endregion

        /// <summary>
        /// Create async request with HttpClient
        /// </summary>
        /// <param name="action">Action only used for logging</param>
        /// <param name="url">Url only used for logging</param>
        /// <param name="token">Authetication token</param>
        /// <param name="request">The request Func</param>
        /// <returns>The result of <see cref="HttpResponseMessage"/></returns>
        private async Task<T> CreateRequest<T>(string action, string url, string token, Func<HttpClient, Task<HttpResponseMessage>> request)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    if (token != null)
                    {
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                    }

                    var httpResponseMessage = await request(client);

                    if (httpResponseMessage.StatusCode.Equals(HttpStatusCode.Unauthorized))
                    {
                        throw new AuthenticationException("Unauthorized request");
                    }

                    var responseString = await httpResponseMessage.Content.ReadAsStringAsync();

                    _logService.Log(action + " response - URL: " + url + " RESPONSE: " + responseString,
                        LogLevel.Debug, typeof(RequestService));

                    return await httpResponseMessage.Content.ReadAsAsync<T>();
                }
            }
            catch (AuthenticationException exception)
            {
                _logService.Log(exception, "FAILED " + action + " request - URL: " + url + ", " + exception.Message, LogLevel.Error,
                    typeof(RequestService));
                throw;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED " + action + " request - URL: " + url, LogLevel.Error,
                    typeof(RequestService));
                throw;
            }
        }

    }
}
