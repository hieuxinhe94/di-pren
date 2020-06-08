using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptions
{
    public class BusinessSubscription
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("orgName")]
        public string OrgName { get; set; }

        [JsonProperty("organizationNumber")]
        public string OrganizationNumber { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("entitlementId")]
        public string EntitlementId { get; set; }

        [JsonProperty("masterUserId")]
        public string MasterUserId { get; set; }

        [JsonProperty("businessSubscriptionDefinitionId")]
        public string BusinessSubscriptionDefinitionId { get; set; }

        [JsonProperty("registrationCode")]
        public string RegistrationCode { get; set; }

        [JsonProperty("codeExpires")]
        public string CodeExpires { get; set; }

        [JsonProperty("firstDefinition")]
        public string FirstDefinition { get; set; }
    }
}
