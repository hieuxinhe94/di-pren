using System;

namespace Di.Subscription.Logic.HolidayStop.Modifiers
{
    internal interface IHolidayStopCreator
    {
        string CreateHolidayStop(
           DateTime startDate,
           DateTime endDate,
           long subscriptionNumber);
    }
}