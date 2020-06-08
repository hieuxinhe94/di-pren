using Di.ServicePlus.RestApi.ResponseModels.Entitlement;
using Di.ServicePlus.RestApi.ResponseModels.ExternalIdsResponse;

namespace Di.ServicePlus.RestApi.Requests.Entitlements
{
    public interface IEntitlements
    {
        ExternalIdsResponse GetExternalIds(string token);
        EntitlementResponse GetEntitlement(string entitlementId, string token);
        EntitlementsResponse GetEntitlements(string userId, string token);
        EntitlementResponse ImportEntitlement(string productId, string userId, long externalSubscriberId, long externalSubscriptionId, string token);
        VerifyEntitlementResponse VerifyEntitlement(string externalResourceId, string token);
    }
}