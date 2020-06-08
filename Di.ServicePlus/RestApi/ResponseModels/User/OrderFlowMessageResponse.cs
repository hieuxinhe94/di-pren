using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class OrderFlowMessageResponse : ResponseBase
    {
        [JsonProperty("forceLogin")]
        public bool ForceLogin { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("header")]
        public string Header { get; set; }
    }
}
