using System.Collections.Generic;

namespace Di.Subscription.Logic.HolidayStop.Retrievers
{
    internal interface IHolidayStopRetriever
    {
        IEnumerable<Types.HolidayStopItem> GetHolidayStops(
           long subscriptionNumber,
           int externalNumber);
    }
}