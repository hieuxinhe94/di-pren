using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.HolidayStop;

namespace Bn.Subscription.RestApi.Requests.HolidayStop
{
    public interface IHolidayStop
    {
        Task<ApiResponse<string>> CreateHolidayStopAsync(string brand, long customerNumber, long subscriptionNumber,
            int subscriptionSequenceNumber, DateTime startDate, DateTime stopDate);

        Task<ApiResponse<List<HolidayStopModel>>> GetHolidayStopsAsync(string brand, long customerNumber, long subscriptionNumber,
            int subscriptionSequenceNumber);

        Task<ApiResponse<string>> ChangeHolidayStopAsync(string brand, long customerNumber, long subscriptionNumber,
            int subscriptionSequenceNumber,
            DateTime startDate, DateTime oldStartDate, DateTime stopDate, DateTime oldStopDate);

        Task<ApiResponse<string>> DeleteHolidayStopAsync(string brand, long customerNumber, long subscriptionNumber,
            int subscriptionSequenceNumber,
            DateTime startDate, DateTime stopDate);
    }
}
