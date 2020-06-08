using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.ExternalIdsResponse
{
    public class ExternalIdsResponse : ResponseBase
    {
        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("externalIds")]
        [JsonConverter(typeof(SingleOrArrayConverter<ExternalId>))]
        public List<ExternalId> ExternalIds { get; set; }
    }
}
