using System.Collections.Generic;
using System.Globalization;
using System.Web.Script.Serialization;
using Di.Common.WebRequests;
using Di.ServicePlus.RestApi.ResponseModels.Entitlement;
using Di.ServicePlus.RestApi.ResponseModels.ExternalIdsResponse;
using Di.ServicePlus.Utils;

namespace Di.ServicePlus.RestApi.Requests.Entitlements
{
    internal class Entitlements : RequestBase, IEntitlements
    {
        public Entitlements(string servicePlusApiUrl)
            : base(servicePlusApiUrl)
        {
        }

        public Entitlements(string servicePlusApiUrl, IRequestService requestService)
            : base(servicePlusApiUrl, requestService)
        {
        }

        public ExternalIdsResponse GetExternalIds(string token)
        {
            var responseJson = CreateRequestWithToken(RequestVerb.Get, "entitlements/external-ids/", token);

            return responseJson.ConvertServicePlusJsonToObject<ExternalIdsResponse>();
        }

        public EntitlementResponse GetEntitlement(string entitlementId, string token)
        {
            var responseJson = CreateRequestWithToken(RequestVerb.Get, "entitlements/" + entitlementId, token);

            return responseJson.ConvertServicePlusJsonToObject<EntitlementResponse>();
        }

        public EntitlementsResponse GetEntitlements(string userId, string token)
        {
            var responseJsonString = CreateRequestWithToken(RequestVerb.Get, "entitlements", token, "", new Dictionary<string, string> { { "q", "userId_s:" + userId } });

            return responseJsonString.ConvertServicePlusJsonToObject<EntitlementsResponse>();
        }

        public EntitlementResponse ImportEntitlement(string productId, string userId, long externalSubscriberId, long externalSubscriptionId, string token)
        {
            var urlParameters = new Dictionary<string, string>
            {
                {"productId", productId},
                {"userId", userId},
                {"externalSubscriberId", externalSubscriberId.ToString(CultureInfo.InvariantCulture)},
                {"externalSubscriptionId", externalSubscriptionId.ToString(CultureInfo.InvariantCulture)},
                {"createTemp", "true"},
            };

            var responseJson = CreateRequestWithToken(RequestVerb.Post, "entitlements/import", token, contentType: "application/x-www-form-urlencoded", urlParameters: urlParameters);

            return responseJson.ConvertServicePlusJsonToObject<EntitlementResponse>();
        }

        public VerifyEntitlementResponse VerifyEntitlement(string externalResourceId, string token)
        {
            var queryParameters = new Dictionary<string, string>
            {
                {"externalResourceId", externalResourceId}
            };

            var response = CreateRequestWithToken(RequestVerb.Get, "resources/verify-entitlement/", token, queryParameters: queryParameters);

            return response.ConvertServicePlusJsonToObject<VerifyEntitlementResponse>();
        }
    }
}