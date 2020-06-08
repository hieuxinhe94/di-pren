using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.OAuthToken
{
    public class OAuthTokenResponse : ResponseBase
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        [JsonProperty("scope")]
        public string Scope { get; set; }
    }
}
