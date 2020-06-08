using Newtonsoft.Json;

namespace Bn.Subscription.RestApi.ResponseModels.Customer
{
    public class CustomerModel
    {
        [JsonProperty("customerNumber")]
        public long CustomerNumber { get; set; }
        [JsonProperty("companyName")]
        public string CompanyName { get; set; }
        [JsonProperty("companyNumber")]
        public string CompanyNumber { get; set; }
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        [JsonProperty("firstName")]
        public string FirstName { get; set; }
        [JsonProperty("lastName")]
        public string LastName { get; set; }
        [JsonProperty("phone")]
        public string Phone { get; set; }
        [JsonProperty("email")]
        public string Email { get; set; }
        [JsonProperty("pointer")]
        public long Pointer { get; set; }
        [JsonProperty("address")]
        public Address.AddressModel Address { get; set; }
    }
}
