using System.Collections.Generic;
using Di.Common.WebRequests;
using Di.ServicePlus.RestApi.ResponseModels.OAuthToken;
using Di.ServicePlus.Utils;

namespace Di.ServicePlus.RestApi.Requests.OAuth
{
    internal class OAuth : RequestBase, IOAuth
    {
        public OAuth(string servicePlusApiUrl) : base(servicePlusApiUrl)
        {
        }

        public OAuth(string servicePlusApiUrl, IRequestService requestService) : base(servicePlusApiUrl, requestService)
        {
        }

        public OAuthTokenResponse GetSystemUserAccessToken(string clientId, string clientSecret, string grantType)
        {
            var urlParameters = new Dictionary<string, string>
            {
                {"client_id", clientId},
                {"client_secret", clientSecret},
                {"grant_type", grantType}
            };

            var responseJson = CreateRequest(RequestVerb.Post, "oauth/token", urlParameters);

            return responseJson.ConvertServicePlusJsonToObject<OAuthTokenResponse>();
        }
    }
}