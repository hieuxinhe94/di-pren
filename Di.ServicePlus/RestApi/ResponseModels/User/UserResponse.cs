using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class UserResponse : ResponseBase
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("user")]
        public User User { get; set; }
    }
}
