using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class ExternalUserIds
    {
        [JsonProperty("system")]
        public string System { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
