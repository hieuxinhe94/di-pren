using Di.ServicePlus.RestApi.ResponseModels.BizSubscribers;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriberDelete
{
    public class BizPendingSubscriberDeleteResponse : ResponseBase
    {
        [JsonProperty("subscriber")]
        public PendingSubscriber Subscriber { get; set; }
    }
}
