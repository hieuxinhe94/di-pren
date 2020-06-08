using System.Text.RegularExpressions;
using DIClassLib.OneByOne;
using Pren.Web.Business.Detection;

namespace Pren.Web.Business.Address
{
    public class ParService : IAddressService
    {
        private readonly IDetectionHandler _detectionHandler;

        public ParService(IDetectionHandler detectionHandler)
        {
            _detectionHandler = detectionHandler;
        }

        public AddressResult GetAddress(string input)
        {
            var result = new AddressResult();

            if (new Regex(@"(^[\d]{10,12}$)").IsMatch(input))
            {
                input = input.Length == 10 ? "19" + input : input;
                var person = Obo.GetPerson(input);

                if (person != null && !string.IsNullOrEmpty(person.FirstNames))
                {
                    result.FirstNames = string.IsNullOrEmpty(person.GivenNames) ? person.FirstNames : person.GivenNames;
                    result.LastNames = person.LastNames;
                    result.GivenNames = person.GivenNames;
                    result.StreetAddressRaw = person.StreetAddressRaw;
                    result.ZipCode = person.ZipCode;
                    result.City = person.City;
                    result.PhoneMobile = person.PhoneMobile;
                    result.Name = string.Empty;
                    result.CompanyNumber = string.Empty;
                }
                else
                {
                    result.Error = "Vi hittar inga personuppgifter för " + input;
                }
            }
            else if (new Regex(@"(^[\d]{6}(-)[\d]{4}$)").IsMatch(input))
            {
                var company = Obo.GetCompany(input.Replace("-", string.Empty));

                if (company != null && !string.IsNullOrEmpty(company.Name))
                {
                    result.FirstNames = string.Empty;
                    result.LastNames = string.Empty;
                    result.GivenNames = string.Empty;
                    result.PhoneMobile = string.Empty;
                    result.StreetAddressRaw = company.StreetAddressRaw;
                    result.ZipCode = company.ZipCode;
                    result.City = company.City;
                    result.Name = company.Name;
                    result.CompanyNumber = company.CompanyNumber;
                }
                else
                {
                    result.Error = "Vi hittar inga företagsuppgifter för " + input;
                }
            }
            else
            {
                result.Error = "Felaktigt format, ÅÅÅÅMMDDXXXX eller XXXXXX-XXXX";
            }

            return Normalize(result);
        }

        
        /// <summary>
        /// Fixing address from PAR to Kayak-format
        /// </summary>       
        private AddressResult Normalize(AddressResult infoResult)
        {
            if (infoResult.StreetAddressRaw == null)
                return infoResult;

            string streetAddress = string.Empty, streetNo = string.Empty, stairCase = string.Empty, stairs = string.Empty, appNo = string.Empty;

            var nextIsAppNo = false;

            foreach (var arrayItem in infoResult.StreetAddressRaw.Split(' '))
            {
                if (nextIsAppNo)
                {
                    appNo = arrayItem;
                    nextIsAppNo = false;
                    continue;
                }

                if (arrayItem.ToLower().EndsWith("tr"))
                    stairs = arrayItem.ToLower().Replace("tr", string.Empty);
                else if (arrayItem.ToLower().Equals("lgh"))
                    nextIsAppNo = true;
                else if (arrayItem.ToLower().StartsWith("lgh"))
                    appNo = arrayItem.ToLower().Replace("lgh", string.Empty);
                else if (_detectionHandler.IsNumeric(arrayItem) && string.IsNullOrEmpty(streetNo))
                    streetNo = arrayItem;
                else if (arrayItem.Length == 1 && !_detectionHandler.IsNumeric(arrayItem) && string.IsNullOrEmpty(stairCase))
                    stairCase = arrayItem;
                else if (!_detectionHandler.IsNumeric(arrayItem)) //As long it isn't numeric, it must be street address
                    streetAddress += arrayItem + " ";
            }

            infoResult.StreetAddress = streetAddress;
            infoResult.HouseNumber = streetNo;
            infoResult.StairCase = stairCase;
            infoResult.Stairs = stairs;
            infoResult.AppartmentNumber = appNo;

            return infoResult;
        }
    }
}
