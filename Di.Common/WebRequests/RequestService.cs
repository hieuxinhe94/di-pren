using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Di.Common.Logging;

namespace Di.Common.WebRequests
{
    public class RequestService : IRequestService
    {
        private const string DefaultContentType = "application/json";

        private readonly ILogger _logService;

        public RequestService(ILogger logService)
        {
            _logService = logService;
        }

        public string CreateGetRequestWithHeaderParams(string url, Dictionary<string, string> headerParamsDictionary, string contentType = "application/json")
        {
            try
            {
                var httpWebRequest = GetWebRequest(url, RequestMethod.GET, contentType);

                foreach (var headerParam in headerParamsDictionary)
                {
                    httpWebRequest.Headers[headerParam.Key] = headerParam.Value;
                }

                var response = GetResponse(httpWebRequest);

                _logService.Log("GET response - URL: " + url + " RESPONSE: " + response, LogLevel.Debug, typeof(RequestService));

                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED GET request - URL: " + url, LogLevel.Error, typeof(RequestService));
            }

            return string.Empty;
        }

        public string CreateGetRequest(string url, string contentType = DefaultContentType)
        {
            try
            {
                var httpWebRequest = GetWebRequest(url, RequestMethod.GET, contentType);

                var response =  GetResponse(httpWebRequest);
                
                _logService.Log("GET response - URL: " + url + " RESPONSE: " + response, LogLevel.Debug, typeof(RequestService));
                
                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED GET request - URL: " + url, LogLevel.Error, typeof(RequestService));                
            }

            return string.Empty;
        }

        public string CreatePostRequest(string url, string json, string contentType = DefaultContentType)
        {
            try
            {
                var httpWebRequest = GetWebRequest(url, RequestMethod.POST, contentType);

                httpWebRequest = WriteJsonToWebRequestStream(httpWebRequest, json);

                var response = GetResponse(httpWebRequest);

                _logService.Log("POST response - URL: " + url + " JSON: " + json + " RESPONSE: " + response, LogLevel.Debug, typeof(RequestService));
                
                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED POST request - URL: " + url + " JSON: " + json, LogLevel.Error, typeof(RequestService));
            }

            return string.Empty;
        }

        public string CreatePostRequest(string url, Dictionary<string, string> keyValueData, string contentType = DefaultContentType)
        {
            try
            {
                var httpWebRequest = GetWebRequest(url, RequestMethod.POST, contentType);

                httpWebRequest = WriteKeyValueDataToRequestStream(httpWebRequest, keyValueData);

                var response = GetResponse(httpWebRequest);

                _logService.Log("POST response - URL: " + url + " KEYVALUEDATA: " + keyValueData + " RESPONSE: " + response, LogLevel.Debug, typeof(RequestService)); //todo: log keyvaluedata

                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED POST request - URL: " + url + " KEYVALUEDATA: " + keyValueData, LogLevel.Error, typeof(RequestService)); //todo: log keyvaluedata
            }

            return string.Empty;
        }

        public string CreatePutRequest(string url, string json, string contentType = DefaultContentType)
        {
            try
            {
                _logService.Log("PUT request - URL: " + url, LogLevel.Debug, typeof(RequestService));
                var httpWebRequest = GetWebRequest(url, RequestMethod.PUT, contentType);

                httpWebRequest = WriteJsonToWebRequestStream(httpWebRequest, json);

                var response = GetResponse(httpWebRequest);

                _logService.Log("PUT response - URL: " + url + " JSON:" + json + " RESPONSE: " + response, LogLevel.Debug, typeof(RequestService));

                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED PUT request - URL: " + url + " JSON:" + json, LogLevel.Debug, typeof(RequestService));
            }

            return string.Empty;
        }

        public string CreateDeleteRequest(string url, string contentType = DefaultContentType)
        {
            try
            {
                var httpWebRequest = GetWebRequest(url, RequestMethod.DELETE, contentType);

                var response = GetResponse(httpWebRequest);

                _logService.Log("DELETE response - URL: " + url + " RESPONSE: " + response, LogLevel.Debug, typeof(RequestService));

                return response;
            }
            catch (Exception exception)
            {
                _logService.Log(exception, "FAILED DELETE request - URL: " + url, LogLevel.Debug, typeof(RequestService));
            }

            return string.Empty;
        }

        private HttpWebRequest GetWebRequest(string url, RequestMethod method, string contentType = DefaultContentType)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = method.ToString();
            httpWebRequest.ContentType = contentType;
            return httpWebRequest;
        }

        private HttpWebRequest WriteJsonToWebRequestStream(HttpWebRequest httpWebRequest, string json)
        {
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            return httpWebRequest;
        }

        private HttpWebRequest WriteKeyValueDataToRequestStream(HttpWebRequest httpWebRequest, Dictionary<string, string> keyValueData)
        {
            var urlParametersString = string.Empty;

            foreach (var keyValuePair in keyValueData)
            {
                urlParametersString += "&" + keyValuePair.Key + "=" + keyValuePair.Value;
            }

            urlParametersString = urlParametersString.TrimStart('&');

            var data = Encoding.ASCII.GetBytes(urlParametersString);

            using (var stream = httpWebRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            return httpWebRequest;
        }

        private string GetResponse(HttpWebRequest httpWebRequest)
        {
            try
            {
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                return ReadResponseStream(httpResponse.GetResponseStream());
            }
            // If the httpresponsecode is not 200 and for example 500, 404 or 400 a WebException is thrown, we catch it and read the response. Other Exceptions will be thrown
            catch (WebException exception)
            {
                return ReadResponseStream(exception.Response.GetResponseStream());
            }
        }

        private string ReadResponseStream(Stream responseStream)
        {
            if (responseStream == null)
            {
                return string.Empty;
            }

            using (var streamReader = new StreamReader(responseStream))
            {
                return streamReader.ReadToEnd();
            }
        }

        private enum RequestMethod
        {
            // ReSharper disable InconsistentNaming
            GET,
            POST,
            PUT,
            DELETE
            // ReSharper restore InconsistentNaming
        }
    }
}
