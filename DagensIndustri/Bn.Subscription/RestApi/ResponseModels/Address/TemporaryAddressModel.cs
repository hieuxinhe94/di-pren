using System;
using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Address
{
    public class TemporaryAddressModel
    {
        [JsonProperty("address")]
        public AddressModel Address { get; set; }
        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }
        [JsonProperty("stopDate")]
        public DateTime StopDate { get; set; }
    }
}
