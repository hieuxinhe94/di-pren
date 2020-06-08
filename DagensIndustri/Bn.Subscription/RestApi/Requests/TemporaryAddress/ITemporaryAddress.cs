using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Address;

namespace Bn.Subscription.RestApi.Requests.TemporaryAddress
{
    public interface ITemporaryAddress
    {
        Task<ApiResponse<List<TemporaryAddressModel>>> GetTemporaryAddresses(
            string brand, 
            long customerNumber,
            long subscriptionNumber);

        Task<ApiResponse<List<AddressModel>>> GetTemporaryAddressesList(
            string brand,
            long customerNumber);

        Task<ApiResponse<string>> CreateTemporaryAddressChange(
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
            DateTime stopDate);

        Task<ApiResponse<string>> DeleteTemporaryAddressChangeAsync(
            string brand,
            long customerNumber,
            long subscriptionNumber,
            int subscriptionSequenceNumber,
            DateTime startDate,
            DateTime stopDate);

        Task<ApiResponse<string>> ChangeTemporaryAddressChangeAsync(
            string brand,
            long customerNumber,
            long subscriptionNumber,
            int subscriptionSequenceNumber,
            string addressPointer,
            DateTime startDate,
            DateTime oldStartDate,
            DateTime stopDate,
            DateTime oldStopDate);
    }
}