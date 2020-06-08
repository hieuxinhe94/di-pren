using System;
using System.Collections.Generic;
using Di.Subscription.Logic.Address.Types;

namespace Di.Subscription.Logic.Address
{
    public interface IAddressHandler
    {
        string CreateTemporaryAddressChange(
           long customerNumber,
           long subscriptionNumber,
           int externalNumber,
           DateTime startDate,
           DateTime endDate,
           string streetAddress,
           string streetNo,
           string stairCase,
           string floor,
           string apartment,
           string street2,
           string zip);

        string DeleteTemporaryAddress(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            DateTime startDate);

        string ChangeTemporaryAddressChange(long customerNumber, long subscriptionNumber, int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate);

        string DeletePermanentAddressChange(long customerNumber, DateTime startDate);

        IEnumerable<AddressChange> GetTemporaryAddressChanges(long customerNumber, long subscriptionNumber);
        IEnumerable<AddressChange> GetTemporaryAddresses(long customerNumber);

        string CreatePermanentAddressChange(
              long customerNumber,
              DateTime startDate,
              string streetAddress,
              string streetNo,
              string stairCase,
              string floor,
              string apartment,
              string street2,
              string zip);

        IEnumerable<AddressChange> GetPermanentAddressChanges(long customerNumber, long subscriptionNumber);

        string AddAddressChangeFee(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            bool basicAddressChange);
    }
}