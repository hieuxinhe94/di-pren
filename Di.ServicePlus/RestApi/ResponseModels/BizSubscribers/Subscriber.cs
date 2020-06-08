using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscribers
{
    public class Subscriber
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("adminUserId")]
        public string AdminUserId { get; set; }

        [JsonProperty("subUserId")]
        public string SubUserId { get; set; }

        [JsonProperty("subscriptionAdmin")]
        public string SubscriptionAdmin { get; set; }

        [JsonProperty("entitlementId")]
        public string EntitlementId { get; set; }

        [JsonProperty("removal")]
        public string Removal { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
