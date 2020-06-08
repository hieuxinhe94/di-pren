using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bn.Subscription.RestApi.ResponseModels;
using Bn.Subscription.RestApi.ResponseModels.Address;

namespace Bn.Subscription.RestApi.Requests.PermanentAddress
{
    public interface IPermanentAddress
    {
        Task<ApiResponse<string>> CreatePermanentAddressChange(
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
            string apartmentNumber);

        Task<ApiResponse<List<PermanentAddressModel>>> GetPermanenAddresses(string brand, long customerNumber);

        Task<ApiResponse<string>> DeletePermanentAddress(
            string brand,
            long customerNumber,
            string addressPointer,
            DateTime startDate);
    }
}