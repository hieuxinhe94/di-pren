using System;
using Di.Subscription.DataAccess.AddressChange;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal class PermanentAddressCreator : IPermanentAddressCreator
    {
        private readonly IAddressChangeRepository _addressChangeRepository;

        public PermanentAddressCreator(IAddressChangeRepository addressChangeRepository)
        {
            _addressChangeRepository = addressChangeRepository;
        }

        public string CreatePermanentAddressChange(
            long customerNumber, 
            DateTime startDate, 
            string streetAddress, 
            string streetNo,
            string stairCase, 
            string floor,
            string apartment, 
            string street2, 
            string zip)
        {
            var permanentAddress = new Types.PermanentAddress(
                customerNumber,
                startDate,
                streetAddress,
                streetNo,
                stairCase,
                floor,
                apartment,
                street2,
                zip);

            return CreatePermanentAddressChange(permanentAddress);
        }

        private string CreatePermanentAddressChange(Types.PermanentAddress permanentAddress)
        {
            return _addressChangeRepository.CreatePermanentAddressChange(
                permanentAddress.UserId,
                permanentAddress.CustomerNumber,
                permanentAddress.StreetAddress,
                permanentAddress.StreetNumber,
                permanentAddress.StairCase,
                permanentAddress.Floor,
                permanentAddress.Apartment,
                permanentAddress.Street2,
                permanentAddress.Street3,
                permanentAddress.CountryCode,
                permanentAddress.Zip,
                permanentAddress.StartDate,
                permanentAddress.TempName1,
                permanentAddress.TempName2,
                permanentAddress.ReceiveType,
                permanentAddress.ChangeImmediately);
        }
    }
}