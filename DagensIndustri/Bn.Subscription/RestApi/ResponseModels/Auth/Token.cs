﻿using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Auth
{
    public class Token
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; } 

        [JsonProperty("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonProperty(".expires")]
        public DateTime Expires { get; set; }

        [JsonProperty(".issued")]
        public DateTime Issued { get; set; }
    }
}
