using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscribers
{
    public class PendingSubscriber
    {
        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("adminUserId")]
        public string AdminUserId { get; set; }

        [JsonProperty("subUserId")]
        public string SubUserId { get; set; }

        [JsonProperty("entitlementId")]
        public string EntitlementId { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("inviteAt")]
        public string InviteAt { get; set; }

        [JsonProperty("inviteCode")]
        public string InviteCode { get; set; }
    }
}
