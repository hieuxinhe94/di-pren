using System;

namespace Di.Subscription.Logic.Address.Modifiers
{
    internal interface ITemporaryAddressChanger
    {
        string ChangeTemporaryAddressChangeDates(long customerNumber, long subscriptionNumber,
            int externalNumber,
            DateTime oldStartDate, DateTime newStartDate, DateTime newEndDate, bool invoice, DateTime cusAddrEndDate);
    }
}