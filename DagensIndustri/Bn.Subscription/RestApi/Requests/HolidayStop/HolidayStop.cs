using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.HolidayStop;
using Di.Common.Cache;

namespace Bn.Subscription.RestApi.Requests.HolidayStop
{
    public class HolidayStop : RequestBase, IHolidayStop
    {
        public HolidayStop(string apiUrl, string apiClient, string apiSecret) : base(apiUrl, apiClient, apiSecret)
        {

        }

        public HolidayStop(string apiUrl, string apiClient, string apiSecret, IRequestService requestService, IObjectCache objectCache)
            : base(apiUrl, apiClient, apiSecret, requestService, objectCache)
        {
        }

        public async Task<ApiResponse<string>> CreateHolidayStopAsync(string brand, long customerNumber, long subscriptionNumber,
            int subscriptionSequenceNumber, DateTime startDate, DateTime stopDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString()},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"stopDate", stopDate.ToString(CultureInfo.InvariantCulture)}
            };

            return await PostAsync<ApiResponse<string>>(GetHolidayEndPointUrl(brand, customerNumber), parameters);
        }

        public async Task<ApiResponse<List<HolidayStopModel>>> GetHolidayStopsAsync(string brand, long customerNumber, long subscriptionNumber, int subscriptionSequenceNumber)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString(CultureInfo.InvariantCulture)},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString(CultureInfo.InvariantCulture)}
            };

            return await GetAsync<ApiResponse<List<HolidayStopModel>>>(GetHolidayEndPointUrl(brand, customerNumber), parameters);
        }

        public async Task<ApiResponse<string>> ChangeHolidayStopAsync(string brand, long customerNumber, long subscriptionNumber, int subscriptionSequenceNumber,
            DateTime startDate, DateTime oldStartDate, DateTime stopDate, DateTime oldStopDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString(CultureInfo.InvariantCulture)},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString(CultureInfo.InvariantCulture)},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"oldStartDate", oldStartDate.ToString(CultureInfo.InvariantCulture)},
                {"stopDate", stopDate.ToString(CultureInfo.InvariantCulture)},
                {"oldStopDate", oldStopDate.ToString(CultureInfo.InvariantCulture)},
            };

            return await PutAsync<ApiResponse<string>>(GetHolidayEndPointUrl(brand, customerNumber), parameters);
        }

        public async Task<ApiResponse<string>> DeleteHolidayStopAsync(string brand, long customerNumber, long subscriptionNumber, int subscriptionSequenceNumber,
            DateTime startDate, DateTime stopDate)
        {
            var parameters = new Dictionary<string, string>
            {
                {"subscriptionNumber", subscriptionNumber.ToString()},
                {"subscriptionSequenceNumber", subscriptionSequenceNumber.ToString()},
                {"startDate", startDate.ToString(CultureInfo.InvariantCulture)},
                {"stopDate", stopDate.ToString(CultureInfo.InvariantCulture)}
            };

            return await DeleteAsync<ApiResponse<string>>(GetHolidayEndPointUrl(brand, customerNumber), parameters);
        }

        private string GetHolidayEndPointUrl(string brand, long customerNumber)
        {
            return $"api/{brand}/holidaystop/" + customerNumber;
        }
    }
}
