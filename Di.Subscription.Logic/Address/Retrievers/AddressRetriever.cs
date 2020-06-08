using System.Collections.Generic;
using System.Linq;
using Di.Subscription.DataAccess.AddressChange;
using AddressChange = Di.Subscription.Logic.Address.Types.AddressChange;

namespace Di.Subscription.Logic.Address.Retrievers
{
    internal class AddressRetriever : IAddressRetriever
    {
        private readonly IAddressChangeRepository _addressChangeRepository;
        
        public AddressRetriever(IAddressChangeRepository addressChangeRepository)
        {
            _addressChangeRepository = addressChangeRepository;
        }

        public IEnumerable<AddressChange> GetTemporaryAddressChanges(long customerNumber, long subscriptionNumber)
        {
            return _addressChangeRepository.GetAllAddressChanges(customerNumber, subscriptionNumber)
                .Where(addressChange => addressChange.AddrNo != 1 && 
                    (addressChange.ChangeType.ToLower().Equals(AddressConstants.ChangeTypeTemporary.ToLower()) || 
                    addressChange.ChangeType.ToLower().Equals(AddressConstants.ChangeTypeTemporaryCurrent.ToLower())))
                .Select(GetAddressChange);    
        }

        public IEnumerable<AddressChange> GetPermanentAddressChanges(long customerNumber, long subscriptionNumber)
        {
            return _addressChangeRepository.GetAllAddressChanges(customerNumber, subscriptionNumber)
                .Where(addressChange => addressChange.AddrNo == 1)
                .Select(GetAddressChange);  
        }

        public IEnumerable<AddressChange> GetTemporaryAddresses(long customerNumber)
        {
            var onlyThisCountry = string.Empty;
            var onlyValid = "TRUE";

            return _addressChangeRepository.GetTemporaryAddresses(customerNumber, onlyThisCountry, onlyValid)
                .Select(GetAddressChange);
        }

        private AddressChange GetAddressChange(DataAccess.AddressChange.AddressChange addressChange)
        {
            return new AddressChange
                {
                    Id = addressChange.AddrNo + "_" + addressChange.DoorNo + "_" + addressChange.StartDate + "_" + addressChange.EndDate,
                    StartDate = addressChange.StartDate,
                    EndDate = addressChange.EndDate,
                    StreetAddress = addressChange.StreetAddress,
                    StreetNumber = addressChange.StreetNumber,
                    Street1 = addressChange.Street1,
                    Street2 = addressChange.Street2,
                    StairCase = addressChange.StairCase,
                    Apartment = addressChange.Apartment,
                    Zip = addressChange.Zip,
                    City = addressChange.City
                };    
        }
    }
}