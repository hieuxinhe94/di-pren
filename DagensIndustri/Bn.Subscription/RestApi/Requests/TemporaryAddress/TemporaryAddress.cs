using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Address;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.TemporaryAddress
{
    public class TemporaryAddress : RequestBase, ITemporaryAddress
    {
        public TemporaryAddress(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {
        }

        public TemporaryAddress(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache) : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<List<TemporaryAddressModel>>> GetTemporaryAddresses(string brand, long customerNumber, long subscriptionNumber)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()}
            };

            return await GetAsync<ApiResponse<List<TemporaryAddressModel>>>(GetTemporaryAddressEndPointUrl(brand, customerNumber), parameters);
        }

        public async Task<ApiResponse<List<AddressModel>>> GetTemporaryAddressesList(string brand, long customerNumber)
        {
            return await GetAsync<ApiResponse<List<AddressModel>>>($"api/{brand}/temporaryaddress/addresslist/" + customerNumber);
        }

        public async Task<ApiResponse<string>> CreateTemporaryAddressChange(
            string brand,
            long customerNumber,
            long subscriptionNumber,
            int subscriptionSequenceNumber,
            string streetName,
            string streetNumber,
            string zip,
            string city,
            string stairCase,
            string careOf,
            string stairs,
            string apartmentNumber,
            DateTime startDate,
            DateTime stopDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString()},
                {"streetName", streetName.ToString(CultureInfo.InvariantCulture)},
                {"streetNumber", streetNumber.ToString(CultureInfo.InvariantCulture)},
                {"zip", zip.ToString(CultureInfo.InvariantCulture)},
                {"city", city.ToString(CultureInfo.InvariantCulture)},
                {"stairCase", stairCase.ToString(CultureInfo.InvariantCulture)},
                {"careOf", careOf.ToString(CultureInfo.InvariantCulture)},
                {"stairs", stairs.ToString(CultureInfo.InvariantCulture)},
                {"apartmentNumber", apartmentNumber.ToString(CultureInfo.InvariantCulture)},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"stopDate", stopDate.ToString(CultureInfo.InvariantCulture)}
            };

            return await PostAsync<ApiResponse<string>>(GetTemporaryAddressEndPointUrl(brand, customerNumber), parameters);
        }

        public async Task<ApiResponse<string>> DeleteTemporaryAddressChangeAsync(
            string brand, 
            long customerNumber, 
            long subscriptionNumber,
            int subscriptionSequenceNumber, 
            DateTime startDate, 
            DateTime stopDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString()},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"stopDate", stopDate.ToString(CultureInfo.InvariantCulture)}
            };

            return await DeleteAsync<ApiResponse<string>>(GetTemporaryAddressEndPointUrl(brand, customerNumber), parameters);
        }

        public async Task<ApiResponse<string>> ChangeTemporaryAddressChangeAsync(
            string brand, 
            long customerNumber, 
            long subscriptionNumber,
            int subscriptionSequenceNumber, 
            string addressPointer,
            DateTime startDate, 
            DateTime oldStartDate, 
            DateTime stopDate, 
            DateTime oldStopDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString()},
                {"addressPointer", addressPointer},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"oldStartDate", oldStartDate.ToString(CultureInfo.InvariantCulture)},
                {"stopDate", stopDate.ToString(CultureInfo.InvariantCulture)},
                {"oldStopDate", oldStopDate.ToString(CultureInfo.InvariantCulture)}
            };

            return await PutAsync<ApiResponse<string>>(GetTemporaryAddressEndPointUrl(brand, customerNumber), parameters);
        }

        private string GetTemporaryAddressEndPointUrl(string brand, long customerNumber)
        {
            return $"api/{brand}/temporaryaddress/" + customerNumber;
        }
    }
}