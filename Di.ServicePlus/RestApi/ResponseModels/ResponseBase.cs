using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels
{
    public class ResponseBase
    {
        [JsonProperty("httpResponseCode")]
        public string HttpResponseCode { get; set; }

        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("errorCode")]
        public string ErrorCode { get; set; }

        [JsonProperty("errorMsg")]
        public string ErrorMessage { get; set; }
    }
}
