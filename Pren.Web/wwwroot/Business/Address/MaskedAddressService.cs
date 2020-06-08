using System.IO;
using System.Xml.Serialization;
using Di.Common.Security.Encryption;
using Pren.Web.Business.Configuration;

namespace Pren.Web.Business.Address
{
    public class MaskedAddressService : IMaskedAddressService
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ISiteSettings _siteSettings;

        public MaskedAddressService(ICryptographyService cryptographyService, ISiteSettings siteSettings)
        {
            _cryptographyService = cryptographyService;
            _siteSettings = siteSettings;
        }

        public MaskedAddressResult MaskInfo(MaskedAddressResult addressResult)
        {
            var masked = new MaskedAddressResult
            {
                FirstNames = MaskString(addressResult.FirstNames),
                GivenNames = MaskString(addressResult.GivenNames),
                LastNames = MaskString(addressResult.LastNames),
                StreetAddressRaw = string.Empty,
                StreetAddress = MaskString(addressResult.StreetAddress),
                HouseNumber = MaskString(addressResult.HouseNumber),
                Stairs = MaskString(addressResult.Stairs),
                StairCase = MaskString(addressResult.StairCase),
                AppartmentNumber = MaskString(addressResult.AppartmentNumber),
                ZipCode = MaskString(addressResult.ZipCode),
                City = MaskString(addressResult.City),
                PhoneMobile = addressResult.PhoneMobile,
                Name = addressResult.Name,
                CompanyNumber = addressResult.CompanyNumber,
                Error = addressResult.Error,
                OriginalInfo = EncryptAddressResult(addressResult) // Add the original addressResult serialized and encrypted
            };

            return masked;
        }

        private string MaskString(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return "";
            }

            if (value.Length == 1)
            {
                return value;
            }

            var trimmed = value.Trim();

            if (trimmed.Contains(" "))
            {
                var maskedStringWithSpaces = "";
                var separated = trimmed.Split(' ');
                foreach (var word in separated)
                {
                    maskedStringWithSpaces += MaskString(word) + " ";
                }

                return maskedStringWithSpaces.Trim();
            }

            return trimmed.Substring(0, 1) + new string('*', trimmed.Length - 1);
        }

        private string EncryptAddressResult(MaskedAddressResult maskedAddressResult)
        {
            var xmlSerializer = new XmlSerializer(maskedAddressResult.GetType());
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, maskedAddressResult);
                return _cryptographyService.EncryptString(stringWriter.ToString(), _siteSettings.CryptoKeyUserId,_siteSettings.CryptoIvUserId);
            }
        }

        public MaskedAddressResult DecryptAddressResult(string encryptedAddressResult)
        {
            if (encryptedAddressResult == null)
                return null;

            var xmlSerializer = new XmlSerializer(typeof(MaskedAddressResult));
            object result;
            var decrypted = _cryptographyService.DecryptString(encryptedAddressResult, _siteSettings.CryptoKeyUserId, _siteSettings.CryptoIvUserId);
            using (var stringReader = new StringReader(decrypted))
            {
                result = xmlSerializer.Deserialize(stringReader);
            }

            return result as MaskedAddressResult;
        }
    }
}
