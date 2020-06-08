using System;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal interface ITemporaryAddressRemover
    {
        string DeleteTemporaryAddress(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            DateTime startDate);
    }
}