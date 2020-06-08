using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Issue
{
    public class IssueDateModel
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }
    }
}
