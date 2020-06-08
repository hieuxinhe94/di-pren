using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscribers
{
    public class InvitedBizSubscriberResponse : ResponseBase
    {
        [JsonProperty("subscriber")]
        public PendingSubscriber Subscriber { get; set; }
    }
}
