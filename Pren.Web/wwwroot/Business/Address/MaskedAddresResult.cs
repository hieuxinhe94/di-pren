
namespace Pren.Web.Business.Address
{
    public class MaskedAddressResult : AddressResult
    {
        public MaskedAddressResult()
        {
            
        }
        public MaskedAddressResult(AddressResult addressResult)
        {
            FirstNames = addressResult.FirstNames;
            GivenNames = addressResult.GivenNames;
            LastNames = addressResult.LastNames;
            StreetAddressRaw = addressResult.StreetAddressRaw;
            StreetAddress = addressResult.StreetAddress;
            HouseNumber = addressResult.HouseNumber;
            Stairs = addressResult.Stairs;
            StairCase = addressResult.StairCase;
            AppartmentNumber = addressResult.AppartmentNumber;
            ZipCode = addressResult.ZipCode;
            City = addressResult.City;
            PhoneMobile = addressResult.PhoneMobile;
            Name = addressResult.Name;
            CompanyNumber = addressResult.CompanyNumber;
            Error = addressResult.Error;
        }

        public string OriginalInfo { get; set; }

    }
}