using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionCount
{
    public class BizSubscriptionCountResponse : ResponseBase
    {
        [JsonProperty("numOfSubscribers")]
        public string NumOfSubscribers { get; set; }

        [JsonProperty("activeSubscribers")]
        public string ActiveSubscribers { get; set; }

        [JsonProperty("pendingSubscribers")]
        public string PendingSubscribers { get; set; }
    }
}
