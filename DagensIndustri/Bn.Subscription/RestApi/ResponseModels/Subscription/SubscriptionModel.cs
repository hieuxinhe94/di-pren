using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Subscription
{
    public class SubscriptionModel
    {
        [JsonProperty("subscriptionNumber")]
        public long SubscriptionNumber { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("endDate")]
        public DateTime EndDate { get; set; }
        [JsonProperty("subscriptionState")]
        public string SubscriptionState { get; set; }
        [JsonProperty("subscriptionKind")]
        public string SubscriptionKind { get; set; }
        [JsonProperty("pointer")]
        public long Pointer { get; set; }
        [JsonProperty("isDigital")]
        public bool IsDigital { get; set; }
        [JsonProperty("productName")]
        public string ProductName { get; set; }
        [JsonProperty("hasRenewal")]
        public bool HasRenewal { get; set; }
        [JsonProperty("sequenceNumber")]
        public int SequenceNumber { get; set; }
        [JsonProperty("nextIssueDate")]
        public DateTime NextIssueDate { get; set; }
    }
}
