using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.ExternalIdsResponse
{
    public class ExternalId
    {
        [JsonProperty("externalSubscriberId")]
        public string ExternalSubscriberId { get; set; }

        [JsonProperty("externalProductId")]
        public string ExternalProductId { get; set; }

        [JsonProperty("externalSubscriptionIds")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> ExternalSubscriptionIds { get; set; } 
    }
}
