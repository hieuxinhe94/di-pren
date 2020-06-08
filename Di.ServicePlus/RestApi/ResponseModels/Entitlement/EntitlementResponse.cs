using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.Entitlement
{
    public class EntitlementResponse : ResponseBase
    {
        [JsonProperty("entitlement")]
        public Entitlement Entitlement { get; set; }
    }
}
