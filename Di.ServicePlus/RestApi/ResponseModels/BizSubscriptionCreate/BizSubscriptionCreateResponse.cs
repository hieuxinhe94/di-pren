using Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionCreate
{
    public class BizSubscriptionCreateResponse : ResponseBase
    {
        [JsonProperty("businessSubscription")]
        public BusinessSubscription BusinessSubscription { get; set; }
    }
}
