using System;
using Di.Subscription.DataAccess.AddressChange;

namespace Di.Subscription.Logic.Address.Modifiers
{
    public class TemporaryAddressChanger : ITemporaryAddressChanger
    {
        private readonly IAddressChangeRepository _addressChangeRepository;

        public TemporaryAddressChanger(IAddressChangeRepository addressChangeRepository)
        {
            _addressChangeRepository = addressChangeRepository;
        }

        public string ChangeTemporaryAddressChangeDates(long customerNumber, long subscriptionNumber, int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate)
        {
            return _addressChangeRepository.ChangeTemporaryAddressChangeDates(SubscriptionConstants.DefaultUserId,
                customerNumber, subscriptionNumber, externalNumber, oldStartDate, newStartDate, newEndDate, invoice,
                cusAddrEndDate, SubscriptionConstants.DefaultRecieveType);
        }
    }
}