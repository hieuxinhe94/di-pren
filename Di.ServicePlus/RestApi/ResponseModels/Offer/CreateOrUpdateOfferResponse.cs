using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.Offer
{
    public class CreateOrUpdateOfferResponse : ResponseBase
    {
        [JsonProperty("userOffer")]
        public UserOffer Offer { get; set; }
    }
}
