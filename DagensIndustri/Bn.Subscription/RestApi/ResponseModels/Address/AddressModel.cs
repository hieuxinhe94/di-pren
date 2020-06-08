using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Address
{
    public class AddressModel
    {
        [JsonProperty("pointer")]
        public string Pointer { get; set; }
        [JsonProperty("streetName")]
        public string StreetName { get; set; }
        [JsonProperty("streetNumber")]
        public string StreetNumber { get; set; }
        [JsonProperty("stairCase")]
        public string StairCase { get; set; }
        [JsonProperty("stairs")]
        public string Stairs { get; set; }
        [JsonProperty("careOf")]
        public string CareOf { get; set; }
        [JsonProperty("zip")]
        public string Zip { get; set; }
        [JsonProperty("city")]
        public string City { get; set; }
        [JsonProperty("apartmentNumber")]
        public string ApartmentNumber { get; set; }
    }
}
