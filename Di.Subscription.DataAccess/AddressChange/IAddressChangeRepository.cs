using System;
using System.Collections.Generic;

namespace Di.Subscription.DataAccess.AddressChange
{
    public interface IAddressChangeRepository
    {
        string ChangeTemporaryAddressChangeDates(string userId, long customerNumber, long subscriptionNumber,
            int externalNumber, DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice,
            DateTime cusAddrEndDate, string receiveType);

        string CreateTemporaryAddressChange(
            string userId, 
            long customerNumber, 
            long subscriptionNumber, 
            int externalNumber,
            string streetAddress, 
            string streetNo, 
            string stairCase, 
            string floor,
            string apartment, 
            string street2, 
            string street3,
            string countryCode, 
            string zip,
            DateTime startDate, 
            DateTime endDate, 
            string name1, 
            string name2, 
            string invoiceToTemporaryAddress,
            string paperCode, 
            string receiveType, 
            bool saveAllPackageSubs);

        string DeleteTemporaryAddressChange(
            string userId,
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            DateTime startDate);

        IEnumerable<AddressChange> GetAllAddressChanges(long customerNumber, long subscriptionNumber);

        string CreatePermanentAddressChange(
            string userId,
            long customerNumber,
            string streetAddress,
            string streetNo,
            string stairCase,
            string floor,
            string apartment,
            string street2,
            string street3,
            string countryCode,
            string zip,
            DateTime startDate,
            string tempName1,
            string tempName2,
            string receiveType,
            bool changeImmediately);

        string DeletePermanentAddressChange(string userId, long customerNumber, DateTime startDate);

        IEnumerable<AddressChange> GetPermanentAddressChanges(long customerNumber, long subscriptionNumber);

        IEnumerable<AddressChange> GetTemporaryAddresses(long customerNumber, string onlyThisCountry, string onlyValid);

        string AddAddressChangeFee(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            bool basicAddressChange);
    }
}
