using System.Collections.Generic;

namespace Di.Subscription.Logic.Address.Retrievers
{
    internal interface IAddressRetriever
    {
        IEnumerable<Types.AddressChange> GetTemporaryAddressChanges(long customerNumber, long subscriptionNumber);
        IEnumerable<Types.AddressChange> GetPermanentAddressChanges(long customerNumber, long subscriptionNumber);
        IEnumerable<Types.AddressChange> GetTemporaryAddresses(long customerNumber);
    }
}