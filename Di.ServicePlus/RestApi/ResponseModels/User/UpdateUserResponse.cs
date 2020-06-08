using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class UpdateUserResponse : ResponseBase
    {
        [JsonProperty("user")]
        public User UpdatedUser { get; set; }
    }
}
