using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions
{
    public class BizSubscriptionResponse : ResponseBase
    {
        [JsonProperty("businessSubscription")]
        public BusinessSubscription BusinessSubscription { get; set; }
    }
}
