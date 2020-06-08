using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.Entitlement
{
    public class Entitlement
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

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("renewable")]
        public string Renewable { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("productTags")]
        [JsonConverter(typeof(SingleOrArrayConverter<string>))]
        public List<string> ProductTags { get; set; }

        [JsonProperty("validFrom")]
        public string ValidFrom { get; set; }

        [JsonProperty("validTo")]
        public string ValidTo { get; set; }

        [JsonProperty("externalSubscriptionId")]
        public string ExternalSubscriptionId { get; set; }

        [JsonProperty("externalSubscriberId")]
        public string ExternalSubscriberId { get; set; }
    }
}
