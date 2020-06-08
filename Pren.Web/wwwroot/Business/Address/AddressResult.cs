
namespace Pren.Web.Business.Address
{
    public class AddressResult
    {
        public string FirstNames { get; set; }
        public string GivenNames { get; set; }
        public string LastNames { get; set; }

        public string StreetAddressRaw { get; set; }

        public string StreetAddress { get; set; }
        public string HouseNumber { get; set; }
        public string Stairs { get; set; }
        public string StairCase { get; set; }
        public string AppartmentNumber { get; set; }

        public string ZipCode { get; set; }
        public string City { get; set; }

        public string PhoneMobile { get; set; }

        public string Name { get; set; }
        public string CompanyNumber { get; set; }

        public string Error { get; set; }

    }
}