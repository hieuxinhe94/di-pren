﻿using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Reclaim
{
    public class ReclaimTypeModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayText")]
        public string DisplayText { get; set; }
    }
}
