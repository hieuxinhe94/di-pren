using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class CreateUserResponse : ResponseBase
    {
        [JsonProperty("user")]
        public CreatedUser CreatedUser { get; set; }
    }
}
