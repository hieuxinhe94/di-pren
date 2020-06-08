using System;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal interface IPermanentAddressRemover
    {
        string DeletePermanentAddressChange(long customerNumber, DateTime startDate);
    }
}