using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Reclaim
{
    public class ReclaimModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("displayText")]
        public string DisplayText { get; set; }

        [JsonProperty("reclaimDate")]
        public DateTime ReclaimDate { get; set; }

    }
}
