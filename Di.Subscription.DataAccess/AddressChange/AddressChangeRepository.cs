using System;
using System.Collections.Generic;
using System.Linq;
using Di.Common.Conversion;
using Di.Subscription.DataAccess.DataAccess;

namespace Di.Subscription.DataAccess.AddressChange
{
    internal class AddressChangeRepository : IAddressChangeRepository
    {
        private readonly ISubscriptionDataAccess _subscriptionDataAccess;
        private readonly IObjectConverter _objectConverter;

        public AddressChangeRepository(
            ISubscriptionDataAccess subscriptionDataAccess, 
            IObjectConverter objectConverter)
        {
            _subscriptionDataAccess = subscriptionDataAccess;
            _objectConverter = objectConverter;
        }

        public string ChangeTemporaryAddressChangeDates(string userId, long customerNumber, long subscriptionNumber, int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate,
            string receiveType)
        {
            return _subscriptionDataAccess.ChangeTemporaryAddressChangeDates(userId, customerNumber, subscriptionNumber,
                externalNumber, oldStartDate, newStartDate, newEndDate, invoice, cusAddrEndDate, receiveType);
        }

        public string CreateTemporaryAddressChange(string userId, long customerNumber, long subscriptionNumber, int externalNumber,
            string streetAddress, string streetNo, string stairCase, string floor, string apartment, string street2, string street3,
            string countryCode, string zip, DateTime startDate, DateTime endDate, string name1, string name2,
            string invoiceToTemporaryAddress, string paperCode, string receiveType, bool saveAllPackageSubs)
        {
            return _subscriptionDataAccess.CreateTemporaryAddressChange(
                userId,
                customerNumber,
                subscriptionNumber,
                externalNumber,
                streetAddress,
                streetNo,
                stairCase,
                floor,
                apartment,
                street2,
                street3,
                countryCode,
                zip,
                startDate,
                endDate,
                name1,
                name2,
                invoiceToTemporaryAddress,
                paperCode,
                receiveType,
                saveAllPackageSubs);
        }

        public string DeleteTemporaryAddressChange(
            string userId,
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            DateTime startDate)
        {
            return _subscriptionDataAccess.DeleteTemporaryAddressChange(
                userId,
                customerNumber,
                subscriptionNumber,
                externalNumber,
                startDate);
        }

        public IEnumerable<AddressChange> GetAllAddressChanges(long customerNumber, long subscriptionNumber)
        {
            var temporaryAddressChangesDataSet = _subscriptionDataAccess.GetAllAddressChanges(customerNumber, subscriptionNumber);
            return _objectConverter.ConvertFromDataSet<AddressChange>(temporaryAddressChangesDataSet);
        } 

        public string CreatePermanentAddressChange(
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
            bool changeImmediately)
        {
            return _subscriptionDataAccess.CreatePermanentAddressChange(
                userId,
                customerNumber, 
                streetAddress,
                streetNo,
                stairCase,
                floor,
                apartment,
                street2, 
                street3, 
                countryCode,
                zip, 
                startDate, 
                tempName1, 
                tempName2,
                receiveType, 
                changeImmediately);
        }

        public IEnumerable<AddressChange> GetTemporaryAddresses(long customerNumber, string onlyThisCountry, string onlyValid)
        {
            var temporaryAddressesDataSet = _subscriptionDataAccess.GetCustomerTemporaryAddresses(customerNumber, onlyThisCountry, onlyValid);
            var temporaryAddresses = _objectConverter.ConvertFromDataSet<AddressChange>(temporaryAddressesDataSet);

            return temporaryAddresses;
        }

        public string DeletePermanentAddressChange(string userId, long customerNumber, DateTime startDate)
        {
            return _subscriptionDataAccess.DeletePermanentAddressChange(userId,customerNumber,startDate);
        }

        public IEnumerable<AddressChange> GetPermanentAddressChanges(long customerNumber, long subscriptionNumber)
        {
            var permanentAddressChangesDataSet = _subscriptionDataAccess.GetAllAddressChanges(customerNumber, subscriptionNumber);
            var permanentAddressChanges = _objectConverter.ConvertFromDataSet<AddressChange>(permanentAddressChangesDataSet);

            return permanentAddressChanges.Where(t => t.AddrNo == 1);
        }

        public string AddAddressChangeFee(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            bool basicAddressChange)
        {
            return _subscriptionDataAccess.AddAddressChangeFee(
                customerNumber, 
                subscriptionNumber, 
                externalNumber,
                basicAddressChange);
        }
    }
}
