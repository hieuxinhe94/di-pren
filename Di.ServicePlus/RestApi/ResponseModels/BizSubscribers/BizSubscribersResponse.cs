using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscribers
{
    public class BizSubscribersResponse : ResponseBase
    {
        [JsonProperty("numItems")]
        public string NumberOfItems { get; set; }

        [JsonProperty("startIndex")]
        public string StartIndex { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("subscribers")]
        [JsonConverter(typeof(SingleOrArrayConverter<Subscriber>))]
        public List<Subscriber> Subscribers { get; set; }
    }
}
