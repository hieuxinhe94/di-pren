using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscribers
{
    public class PendingBizSubscribersResponse : ResponseBase
    {
        [JsonProperty("numItems")]
        public string NumberOfItems { get; set; }

        [JsonProperty("startIndex")]
        public string StartIndex { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("subscribers")]
        [JsonConverter(typeof(SingleOrArrayConverter<PendingSubscriber>))]
        public List<PendingSubscriber> Subscribers { get; set; }
    }
}
