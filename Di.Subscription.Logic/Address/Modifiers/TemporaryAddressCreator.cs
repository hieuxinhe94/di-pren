using System;
using Di.Subscription.DataAccess.AddressChange;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal class TemporaryAddressCreator : ITemporaryAddressCreator
    {
        private readonly IAddressChangeRepository _addressChangeRepository;

        public TemporaryAddressCreator(IAddressChangeRepository addressChangeRepository)
        {
            _addressChangeRepository = addressChangeRepository;
        }

        public string CreateTemporaryAddressChange(
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
            string zip)
        {
            var temporaryAddress = new Types.TemporaryAddress(
                customerNumber,
                subscriptionNumber,
                externalNumber,
                startDate,
                endDate,
                streetAddress,
                streetNo,
                stairCase,
                floor,
                apartment,
                street2,
                zip);

            return CreateTemporaryAddressChange(temporaryAddress);

        }

        public string AddAddressChangeFee(long customerNumber, long subscriptionNumber, int externalNumber, bool basicAddressChange)
        {
            return _addressChangeRepository.AddAddressChangeFee(
                customerNumber, 
                subscriptionNumber, 
                externalNumber,
                basicAddressChange);
        }

        private string CreateTemporaryAddressChange(Types.TemporaryAddress temporaryAddress)
        {
            return _addressChangeRepository.CreateTemporaryAddressChange(
                temporaryAddress.UserId, 
                temporaryAddress.CustomerNumber,
                temporaryAddress.SubscriptionNumber, 
                temporaryAddress.ExternalNumber,
                temporaryAddress.StreetAddress, 
                temporaryAddress.StreetNumber,
                temporaryAddress.StairCase, 
                temporaryAddress.Floor,
                temporaryAddress.Apartment, 
                temporaryAddress.Street2,
                temporaryAddress.Street3, 
                temporaryAddress.CountryCode, 
                temporaryAddress.Zip,
                temporaryAddress.StartDate, 
                temporaryAddress.EndDate, 
                temporaryAddress.Name1,
                temporaryAddress.Name2, 
                temporaryAddress.InvoiceToTemporaryAddress,
                temporaryAddress.PaperCode, 
                temporaryAddress.ReceiveType, 
                temporaryAddress.SaveAllPackageSubs);        
        }

    }
}