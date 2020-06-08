using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Address;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.PermanentAddress
{
    public class PermanentAddress : RequestBase, IPermanentAddress
    {
        public PermanentAddress(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {
        }

        public PermanentAddress(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache) : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<string>> CreatePermanentAddressChange(
            string brand, 
            long customerNumber, 
            DateTime startDate, 
            string streetName,
            string streetNumber, 
            string zip, 
            string city, 
            string stairCase, 
            string careOf, 
            string stairs, 
            string apartmentNumber)
        {
            var parameters = new Dictionary<string, string>
            {
                {"customerNumber", customerNumber.ToString()},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"streetName", streetName.ToString(CultureInfo.InvariantCulture)},
                {"streetNumber", streetNumber.ToString(CultureInfo.InvariantCulture)},
                {"zip", zip.ToString(CultureInfo.InvariantCulture)},
                {"city", city.ToString(CultureInfo.InvariantCulture)},
                {"stairCase", stairCase.ToString(CultureInfo.InvariantCulture)},
                {"careOf", careOf.ToString(CultureInfo.InvariantCulture)},
                {"stairs", stairs.ToString(CultureInfo.InvariantCulture)},
                {"apartmentNumber", apartmentNumber.ToString(CultureInfo.InvariantCulture)}
            };

            return await PostAsync<ApiResponse<string>>($"api/{brand}/permanent/" + customerNumber, parameters);
        }

        public async Task<ApiResponse<List<PermanentAddressModel>>> GetPermanenAddresses(string brand, long customerNumber)
        {
            return await GetAsync<ApiResponse<List<PermanentAddressModel>>>(GetPermanentAddressEndPointUrl(brand, customerNumber));
        }

        public async Task<ApiResponse<string>> DeletePermanentAddress(
            string brand, 
            long customerNumber, 
            string addressPointer, 
            DateTime startDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"addressPointer", addressPointer},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)}
            };

            return await DeleteAsync<ApiResponse<string>>(GetPermanentAddressEndPointUrl(brand, customerNumber), parameters);
        }

        private string GetPermanentAddressEndPointUrl(string brand, long customerNumber)
        {
            return $"api/{brand}/permanentaddress/" + customerNumber;
        }
    }
}