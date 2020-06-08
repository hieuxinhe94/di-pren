using System;
using Newtonsoft.Json;

namespace Pren.Web.Business.Student.ResponseModels
{
    public class StudentResponse
    {
        [JsonProperty("Address")]
        public string Address { get; set; }

        [JsonProperty("CO")]
        public string Co { get; set; }

        [JsonProperty("Cardnumber")]
        public string CardNumber { get; set; }

        [JsonProperty("City")]
        public string City { get; set; }

        [JsonProperty("Country")]
        public string Country { get; set; }

        [JsonProperty("DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [JsonProperty("Email")]
        public string Email { get; set; }

        [JsonProperty("Firstname")]
        public string FirstName { get; set; }

        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Lastname")]
        public string LastName { get; set; }

        [JsonProperty("MobilePhone")]
        public string MobilePhone { get; set; }

        [JsonProperty("Password")]
        public string Password { get; set; }

        [JsonProperty("PostalCode")]
        public string PostalCode { get; set; }

        [JsonProperty("ReturnValue")]
        public string ReturnValue { get; set; }

        [JsonProperty("SocialSecurityNumber")]
        public string SocialSecurityNumber { get; set; }

        [JsonProperty("TravelDiscountType")]
        public int TravelDiscountType { get; set; }

        [JsonProperty("ValidTo")]
        public DateTime ValidTo { get; set; }        
    }
}