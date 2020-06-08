using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class UsersBipResponse : ResponseBase
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("user")]
        [JsonConverter(typeof(SingleOrArrayConverter<User>))]
        public List<User> Users { get; set; }

        [JsonProperty("bipAccount")]
        public User BipAccount { get; set; }
    }
}
