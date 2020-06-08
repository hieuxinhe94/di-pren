using Di.ServicePlus.RestApi.ResponseModels.BizSubscribers;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriberDelete
{
    public class MarkBizSubscriberForRemovalResponse : ResponseBase
    {
        [JsonProperty("subscriber")]
        public Subscriber Subscriber { get; set; }
    }
}
