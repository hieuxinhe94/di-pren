using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class SearchUserResponse : ResponseBase
    {
        [JsonProperty("numItems")]
        public string NumItems { get; set; }

        [JsonProperty("startIndex")]
        public string StartIndex { get; set; }

        [JsonProperty("totalItems")]
        public string TotalItems { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("users")]
        [JsonConverter(typeof(SingleOrArrayConverter<User>))]
        public List<User> Users { get; set; }
    }
}
