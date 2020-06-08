using System.Collections.Generic;
using Di.Common.Utils;
using Newtonsoft.Json;

namespace Di.ServicePlus.RestApi.ResponseModels.User
{
    public class User
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

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("active")]
        public string Active { get; set; }

        [JsonProperty("hasEntitlement")]
        public string HasEntitlement { get; set; }

        [JsonProperty("defaultPaymentMethod")]
        public string DefaultPaymentMethod { get; set; }

        [JsonProperty("externalUserIds")]
        [JsonConverter(typeof(SingleOrArrayConverter<ExternalUserIds>))]
        public List<ExternalUserIds> ExternalUserIds { get; set; }
    }
}
