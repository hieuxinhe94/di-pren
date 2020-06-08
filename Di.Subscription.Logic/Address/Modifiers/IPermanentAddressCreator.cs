using System;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal interface IPermanentAddressCreator
    {
        string CreatePermanentAddressChange(
            long customerNumber,
            DateTime startDate,
            string streetAddress,
            string streetNo,
            string stairCase,
            string floor,
            string apartment,
            string street2,
            string zip);
    }
}