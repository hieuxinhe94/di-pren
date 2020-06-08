using System;
using Di.Subscription.DataAccess.AddressChange;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal class TemporaryAddressRemover : ITemporaryAddressRemover
    {
        private readonly IAddressChangeRepository _addressChangeRepository;

        public TemporaryAddressRemover(IAddressChangeRepository addressChangeRepository)
        {
            _addressChangeRepository = addressChangeRepository;
        }

        public string DeleteTemporaryAddress(
            long customerNumber, 
            long subscriptionNumber, 
            int externalNumber,
            DateTime startDate)
        {
            return _addressChangeRepository.DeleteTemporaryAddressChange(
                SubscriptionConstants.DefaultUserId,
                customerNumber,
                subscriptionNumber,
                externalNumber,
                startDate);
        }
    }
}