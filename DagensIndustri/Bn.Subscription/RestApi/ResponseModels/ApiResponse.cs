using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels
{
    public class ApiResponse<T>
    {
        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}
