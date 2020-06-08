using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels
{
    public class SolrQueryResponseBase : ResponseBase
    {
        [JsonProperty("numItems")]
        public string NumberOfItems { get; set; }

        [JsonProperty("startIndex")]
        public string StartIndex { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }
    }
}
