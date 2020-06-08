using System.Collections.Generic;

namespace Di.Common.WebRequests
{
    public interface IRequestService
    {
        string CreateGetRequestWithHeaderParams(string url, Dictionary<string, string> headerParamsDictionary, string contentType = "application/json");
        string CreateGetRequest(string url, string contentType = "application/json");
        string CreatePostRequest(string url, string json, string contentType = "application/json");
        string CreatePostRequest(string url, Dictionary<string, string> keyValueData, string contentType = "application/json");
        string CreatePutRequest(string url, string json, string contentType = "application/json");
        string CreateDeleteRequest(string url, string contentType = "application/json");
    }
}
