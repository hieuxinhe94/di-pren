using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions
{
    public class BizSubscriptionsResponse : ResponseBase
    {
        [JsonProperty("numItems")]
        public string NumItems { get; set; }

        [JsonProperty("startIndex")]
        public string StartIndex { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("businessSubscriptions")]
        [JsonConverter(typeof(SingleOrArrayConverter<BusinessSubscription>))]
        public List<BusinessSubscription> BusinessSubscriptions { get; set; }
    }
}
