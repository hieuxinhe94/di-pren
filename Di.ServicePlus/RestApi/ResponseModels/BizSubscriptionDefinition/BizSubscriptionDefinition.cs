using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.BizSubscriptionDefinition
{
    public class BizSubscriptionDefinition
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

        [JsonProperty("externalProductCode")]
        public string ExternalProductCode { get; set; }

        [JsonProperty("minQuantity")]
        public string MinQuantity { get; set; }

        [JsonProperty("maxQuantity")]
        public string MaxQuantity { get; set; }
    }
}
