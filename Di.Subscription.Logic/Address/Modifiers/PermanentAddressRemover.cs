using System;
using Di.Subscription.DataAccess.AddressChange;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal class PermanentAddressRemover : IPermanentAddressRemover
    {
        private readonly IAddressChangeRepository _addressChangeRepository;

        public PermanentAddressRemover(IAddressChangeRepository addressChangeRepository)
        {
            _addressChangeRepository = addressChangeRepository;
        }

        public string DeletePermanentAddressChange(long customerNumber, DateTime startDate)
        {
            return _addressChangeRepository.DeletePermanentAddressChange(SubscriptionConstants.DefaultUserId, customerNumber, startDate);
        }
    }
}