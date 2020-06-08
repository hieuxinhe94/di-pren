using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.Entitlement
{
    public class VerifyEntitlementResponse : ResponseBase
    {
        [JsonProperty("entitled")]
        public bool Entitled { get; set; }
    }
}
