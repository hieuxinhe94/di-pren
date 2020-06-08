using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.Offer
{
    public class UserOffer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("brandId")]
        public string BrandId { get; set; }

        [JsonProperty("offerType")]
        public string OfferType { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("subscriptionLength")]
        public string SubscriptionLength { get; set; }

        [JsonProperty("forceDisplayed")]
        public string ForceDisplayed { get; set; }

        [JsonProperty("accepted")]
        public string Accepted { get; set; }      
    }
}