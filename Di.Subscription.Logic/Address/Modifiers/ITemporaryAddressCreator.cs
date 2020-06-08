using System;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal interface ITemporaryAddressCreator
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

        string AddAddressChangeFee(
            long customerNumber,
            long subscriptionNumber,
            int externalNumber,
            bool basicAddressChange);
    }

}